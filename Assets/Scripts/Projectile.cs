using UnityEngine;

// БАЗОВЫЙ класс всех снарядов. Наследники: Snowball, BossShot, Icicle.
// Скорость задаёт тот, кто создал снаряд (из своих данных: PlayerStats, EnemyStats, BossStats, ItemValues).
[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    public float speed;      // ставит стрелок сразу после Instantiate
    public float lifeTime;   // через сколько секунд снаряд исчезает (в префабе)

    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;                 // рыбки, порталы и т.п. не мешают
        if (HitSomething(other)) Destroy(gameObject);
    }

    // true = снаряд исчезает
    protected abstract bool HitSomething(Collider other);
}
