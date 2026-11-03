using UnityEngine;

// БАЗОВЫЙ класс всех снарядов (наследование, A6). Наследники: Snowball, BossShot, Icicle.
// Общее: полёт с начальной скоростью, исчезновение через lifeTime, реакция на триггер.
// Что делать при попадании — решает наследник в HitSomething().
[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 3f;

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
