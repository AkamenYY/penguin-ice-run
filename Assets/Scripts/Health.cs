using System.Collections;
using UnityEngine;

// Здоровье пингвина. Удар → мигание (неуязвимость). 0 жизней → уровень заново.
// Сколько жизней и сколько длится неуязвимость — в ассете Player_Stats.
public class Health : MonoBehaviour
{
    public PlayerStats stats;   // ДАННЫЕ игрока
    public AudioClip hurtClip;
    public int hp;              // текущие жизни (сердца в HUD читают это поле)

    bool invincible;   // пока мигаем — урона нет

    void Awake()
    {
        hp = stats.maxHp;
    }

    public void TakeDamage()
    {
        if (invincible || GameManager.I.Ending) return;

        hp--;   // правило игры: любой удар = минус одно сердце
        GetComponent<AudioSource>().PlayOneShot(hurtClip);

        if (hp <= 0) GameManager.I.Restart();
        else StartCoroutine(Blink());
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, stats.maxHp);
    }

    // корутина: прячем/показываем модель, пока идёт неуязвимость
    IEnumerator Blink()
    {
        invincible = true;
        Renderer[] parts = GetComponentsInChildren<Renderer>();

        for (float t = 0f; t < stats.invincibleTime; t += 0.15f)
        {
            foreach (Renderer r in parts) r.enabled = !r.enabled;
            yield return new WaitForSeconds(0.15f);   // скорость мигания (визуал)
        }

        foreach (Renderer r in parts) r.enabled = true;
        invincible = false;
    }
}
