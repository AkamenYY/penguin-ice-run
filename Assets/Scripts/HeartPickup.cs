using UnityEngine;

// Сердце-аптечка: сколько лечит — в ассете Item_Values.
public class HeartPickup : MonoBehaviour
{
    public AudioClip pickupClip;

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health == null) return;   // это не пингвин

        health.Heal(GameManager.I.items.heartHeal);
        AudioManager.Play(pickupClip);
        Destroy(gameObject);
    }
}
