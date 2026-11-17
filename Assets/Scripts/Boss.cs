using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// БОСС — Король моржей (наследник Enemy).
// Корутина Fight() повторяет по кругу: веер ледяных шаров → пауза → ледяной дождь → пауза.
// Вторая фаза (жизней меньше половины): пауза короче, шаров больше, сосулек больше.
// Все числа — в ассете Boss_Stats.
public class Boss : Enemy
{
    public BossStats bossStats;        // ДАННЫЕ босса
    public Projectile shotPrefab;      // ледяной шар
    public Projectile iciclePrefab;    // сосулька
    public GameObject warningPrefab;   // красная метка «сюда упадёт»
    public Transform shootPoint;       // откуда летят шары
    public GameObject exclamation;     // «!» над головой — предупреждение
    public Image hpBar;                // полоска жизней вверху экрана
    public AudioClip shootClip;

    bool phase2;

    protected override int MaxHp()
    {
        return bossStats.maxHp;
    }

    protected override void Start()
    {
        base.Start();   // жизни и ссылка на игрока (player) — из Enemy
        exclamation.SetActive(false);
        StartCoroutine(Fight());
    }

    void Update()
    {
        hpBar.fillAmount = (float)hp / MaxHp();   // 1 — полная полоска, 0 — пустая
    }

    IEnumerator Fight()
    {
        yield return new WaitForSeconds(bossStats.startDelay);
        while (true)
        {
            yield return FanAttack();
            yield return new WaitForSeconds(phase2 ? bossStats.pausePhase2 : bossStats.pause);
            yield return RainAttack();
            yield return new WaitForSeconds(phase2 ? bossStats.pausePhase2 : bossStats.pause);
        }
    }

    // атака 1: «!» → веер шаров в сторону пингвина
    IEnumerator FanAttack()
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(bossStats.warnTime);
        exclamation.SetActive(false);

        int count = phase2 ? bossStats.fanShotsPhase2 : bossStats.fanShots;
        float spread = phase2 ? bossStats.fanSpreadPhase2 : bossStats.fanSpread;
        Vector3 dir = player.position - shootPoint.position;
        dir.y = 0f;

        for (int i = 0; i < count; i++)
        {
            float angle = -spread / 2f + spread * i / (count - 1);   // от левого края до правого
            Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, angle, 0f);
            Projectile shot = Instantiate(shotPrefab, shootPoint.position, rot);
            shot.speed = bossStats.shotSpeed;
        }
        GetComponent<AudioSource>().PlayOneShot(shootClip);
    }

    // атака 2: красные метки вокруг пингвина → через rainWarnTime падают сосульки
    IEnumerator RainAttack()
    {
        int count = phase2 ? bossStats.iciclesPhase2 : bossStats.icicles;
        float r = bossStats.rainRadius;
        Vector3[] spots = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            spots[i] = player.position + new Vector3(Random.Range(-r, r), 0f, Random.Range(-r, r));
            if (i == 0) spots[i] = player.position;   // первая — прямо под пингвином
            spots[i].y = 0f;
            Destroy(Instantiate(warningPrefab, spots[i], Quaternion.identity), bossStats.rainWarnTime + 0.4f);
        }

        yield return new WaitForSeconds(bossStats.rainWarnTime);   // время отбежать

        for (int i = 0; i < count; i++)
        {
            Projectile icicle = Instantiate(iciclePrefab, spots[i] + Vector3.up * 7f, Quaternion.identity);
            icicle.speed = GameManager.I.items.icicleSpeed;
        }
        GetComponent<AudioSource>().PlayOneShot(shootClip);
    }

    public override void TakeHit()
    {
        base.TakeHit();
        if (!dead && !phase2 && hp <= MaxHp() / 2)   // меньше половины жизней — вторая фаза
        {
            phase2 = true;
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
