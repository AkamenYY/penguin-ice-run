using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// БОСС — Король моржей (наследник Enemy).
// Корутина Fight() повторяет по кругу: веер ледяных шаров → пауза → ледяной дождь → пауза.
// Вторая фаза (жизней меньше половины): пауза короче, шаров больше, сосулек больше.
public class Boss : Enemy
{
    public GameObject shotPrefab;      // ледяной шар
    public GameObject iciclePrefab;    // сосулька
    public GameObject warningPrefab;   // красная метка «сюда упадёт»
    public Transform shootPoint;       // откуда летят шары
    public GameObject exclamation;     // «!» над головой — предупреждение
    public Image hpBar;                // полоска жизней вверху экрана
    public AudioClip shootClip;

    Transform player;
    bool phase2;
    float pause = 2.3f;   // отдых между атаками — время бить босса

    protected override void Start()
    {
        base.Start();   // жизни — из Enemy
        player = GameObject.FindWithTag("Player").transform;
        exclamation.SetActive(false);
        StartCoroutine(Fight());
    }

    void Update()
    {
        hpBar.fillAmount = (float)hp / maxHp;   // 1 — полная полоска, 0 — пустая
    }

    IEnumerator Fight()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            yield return FanAttack();
            yield return new WaitForSeconds(pause);
            yield return RainAttack();
            yield return new WaitForSeconds(pause);
        }
    }

    // атака 1: веер ледяных шаров в сторону пингвина
    IEnumerator FanAttack()
    {
        exclamation.SetActive(true);             // предупреждение
        yield return new WaitForSeconds(0.8f);
        exclamation.SetActive(false);

        int count = phase2 ? 7 : 5;              // сколько шаров
        float spread = phase2 ? 80f : 45f;       // ширина веера, градусов
        Vector3 dir = player.position - shootPoint.position;
        dir.y = 0f;

        for (int i = 0; i < count; i++)
        {
            float angle = -spread / 2f + spread * i / (count - 1);   // от левого края до правого
            Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, angle, 0f);
            Instantiate(shotPrefab, shootPoint.position, rot);
        }
        GetComponent<AudioSource>().PlayOneShot(shootClip);
    }

    // атака 2: ледяной дождь — сначала красные метки, через секунду сосульки
    IEnumerator RainAttack()
    {
        int count = phase2 ? 7 : 4;
        Vector3[] spots = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            spots[i] = player.position + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
            if (i == 0) spots[i] = player.position;   // первая — прямо под пингвином
            spots[i].y = 0f;
            Destroy(Instantiate(warningPrefab, spots[i], Quaternion.identity), 1.4f);
        }

        yield return new WaitForSeconds(1f);   // время отбежать

        for (int i = 0; i < count; i++)
            Instantiate(iciclePrefab, spots[i] + Vector3.up * 7f, Quaternion.identity);
        GetComponent<AudioSource>().PlayOneShot(shootClip);
    }

    // попал снежок: −1 жизнь (из Enemy) + проверка второй фазы
    public override void TakeHit()
    {
        base.TakeHit();
        if (!dead && !phase2 && hp <= maxHp / 2)
        {
            phase2 = true;
            pause = 1.2f;   // атакует чаще
            GameManager.I.ShowMessage("THE KING IS ANGRY!", 2f);
        }
    }

    protected override void Die()
    {
        hpBar.fillAmount = 0f;
        GameManager.I.LevelComplete();   // победа → экран Win
        gameObject.SetActive(false);     // босс исчезает, его атаки останавливаются
    }
}
