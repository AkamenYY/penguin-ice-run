using UnityEngine;

// Ледяные шипы: пока пингвин стоит в них — урон (мигание в Health не даёт бить каждый кадр).
public class Hazard : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null) health.TakeDamage();
    }
}
