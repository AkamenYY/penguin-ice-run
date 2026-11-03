using UnityEngine;

// Снежок пингвина (наследник Projectile): попал во врага — у врага −1 жизнь.
public class Snowball : Projectile
{
    protected override bool HitSomething(Collider other)
    {
        if (other.CompareTag("Player")) return false;   // сквозь себя

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) enemy.TakeHit();
        return true;   // попал во врага или в лёд — снежок исчезает
    }
}
