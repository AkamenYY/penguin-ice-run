using System.Collections;
using UnityEngine;

// Здоровье пингвина: 3 жизни. Удар → мигание (неуязвимость). 0 жизней → уровень заново.
public class Health : MonoBehaviour
{
    public int maxHp = 3;
    public int hp = 3;
    public AudioClip hurtClip;

    bool invincible;   // пока мигаем — урона нет

    public void TakeDamage()
    {
        if (invincible || GameManager.I.Ending) return;

        hp--;
        GetComponent<AudioSource>().PlayOneShot(hurtClip);

        if (hp <= 0) GameManager.I.Restart();
        else StartCoroutine(Blink());
    }

    public void Heal()
    {
        if (hp < maxHp) hp++;
    }

    // корутина: 10 раз прячем/показываем модель с паузой 0.15 сек — 1.5 сек неуязвимости
    IEnumerator Blink()
    {
        invincible = true;
        Renderer[] parts = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < 10; i++)
        {
            foreach (Renderer r in parts) r.enabled = !r.enabled;
            yield return new WaitForSeconds(0.15f);
        }

        foreach (Renderer r in parts) r.enabled = true;
        invincible = false;
    }
}
