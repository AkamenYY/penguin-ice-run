using UnityEngine;

// Сердце-аптечка: +1 жизнь.
public class HeartPickup : MonoBehaviour
{
    public AudioClip pickupClip;

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health == null) return;   // это не пингвин

        health.Heal();
        other.GetComponent<AudioSource>().PlayOneShot(pickupClip);
        Destroy(gameObject);
    }
}
