using UnityEngine;

// БАЗОВЫЙ класс врагов. Наследники: Seal (тюлень) и Boss (Король моржей).
// Общее: жизни, урон пингвину при касании. Как умирать — решает каждый наследник (Die).
public abstract class Enemy : MonoBehaviour
{
    public int maxHp = 1;   // сколько снежков выдерживает

    protected int hp;
    protected bool dead;

    protected virtual void Start()
    {
        hp = maxHp;
    }

    // попал снежок
    public virtual void TakeHit()
    {
        if (dead) return;
        hp--;
        if (hp <= 0)
        {
            dead = true;
            Die();
        }
    }

    protected abstract void Die();   // у каждого врага своя смерть

    // касается пингвина — урон
    void OnCollisionStay(Collision c)
    {
        Health health = c.collider.GetComponent<Health>();
        if (health != null && !dead) health.TakeDamage();
    }
}
