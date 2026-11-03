using UnityEngine;

// Бросок снежка с перезарядкой. Снежок летит туда, куда смотрит пингвин.
public class Thrower : MonoBehaviour
{
    public GameObject snowballPrefab;
    public float cooldown = 0.35f;   // перезарядка, сек
    public AudioClip throwClip;

    float nextThrow;   // время, когда можно кинуть снова

    public void Throw()
    {
        if (Time.time < nextThrow) return;   // ещё перезарядка
        nextThrow = Time.time + cooldown;

        Vector3 pos = transform.position + transform.forward * 0.8f + Vector3.up * 0.7f;   // перед пингвином
        Instantiate(snowballPrefab, pos, transform.rotation);                             // повёрнут как пингвин
        GetComponent<AudioSource>().PlayOneShot(throwClip);
    }
}
