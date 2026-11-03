using UnityEngine;

// Рыбка — коллектибл. Триггер (A4): пингвин вошёл → +1 рыба, звук, рыбка исчезает.
// Крутится сама — Animator с клипом FishSpin (A5).
public class Fish : MonoBehaviour
{
    public AudioClip pickupClip;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.I.AddFish();
        if (pickupClip != null) other.GetComponent<AudioSource>().PlayOneShot(pickupClip);
        Destroy(gameObject);
    }
}
