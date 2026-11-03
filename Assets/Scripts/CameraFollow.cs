using UnityEngine;

// Камера третьего лица: плавно летит сзади-сверху за пингвином и смотрит чуть вперёд.
public class CameraFollow : MonoBehaviour
{
    public Transform target;                              // пингвин
    public Vector3 offset = new Vector3(0f, 5.5f, -7.5f); // насколько сзади и сверху
    public float lookAhead = 3f;                          // смотрим на 3 м вперёд пингвина

    void LateUpdate()
    {
        Vector3 focus = target.position;
        if (focus.y < 0f) focus.y = 0f;   // упал в воду — камера под воду не ныряет

        transform.position = Vector3.Lerp(transform.position, focus + offset, 6f * Time.deltaTime);   // плавно догоняем
        transform.LookAt(focus + Vector3.up + Vector3.forward * lookAhead);
    }
}
