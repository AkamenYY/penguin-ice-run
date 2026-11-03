using UnityEngine;

// Сосулька (наследник Projectile): летит ВНИЗ, бьёт пингвина, разбивается о лёд.
public class Icicle : Projectile
{
    protected override void Start()
    {
        base.Start();                              // всё общее из Projectile
        rb.linearVelocity = Vector3.down * speed;  // но летим вниз
    }

    protected override bool HitSomething(Collider other)
    {
        if (other.GetComponent<Enemy>() != null) return false;   // сквозь босса

        Health health = other.GetComponent<Health>();
        if (health != null) health.TakeDamage();
        return true;
    }
}
