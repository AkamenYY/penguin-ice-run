using UnityEngine;

// Бросок снежка (Instantiate). Перезарядка и скорость снежка — в ассете Player_Stats.
public class Thrower : MonoBehaviour
{
    public PlayerStats stats;   // ДАННЫЕ игрока
    public Projectile snowballPrefab;
    public AudioClip throwClip;

    float nextThrow;   // время, когда можно кинуть снова

    public void Throw()
    {
        if (Time.time < nextThrow) return;   // ещё перезарядка
        nextThrow = Time.time + stats.throwCooldown;

        Vector3 pos = transform.position + transform.forward * 0.8f + Vector3.up * 0.7f;   // перед пингвином
        Projectile ball = Instantiate(snowballPrefab, pos, transform.rotation);            // повёрнут как пингвин
        ball.speed = stats.snowballSpeed;
        AudioManager.Play(throwClip);
    }
}
