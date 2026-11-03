using UnityEngine;

// Ледяной шар босса (наследник Projectile): попал в пингвина — −1 жизнь. Колонны его останавливают.
public class BossShot : Projectile
{
    protected override bool HitSomething(Collider other)
    {
        if (other.GetComponent<Enemy>() != null) return false;   // сквозь самого босса

        Health health = other.GetComponent<Health>();
        if (health != null) health.TakeDamage();
        return true;
    }
}
