using UnityEngine;

// Камера меню / экрана победы: медленно облетает сцену по кругу.
public class MenuCamera : MonoBehaviour
{
    public Vector3 center;
    public float radius = 9f;
    public float height = 3.5f;
    public float speed = 6f;       // градусов в секунду
    public float startAngle = 0f;
    public float lookHeight = 1.2f;   // куда смотрим (выше/ниже центра)

    void Update()
    {
        float angle = startAngle + Time.time * speed;
        transform.position = center + Quaternion.Euler(0f, angle, 0f) * new Vector3(0f, height, -radius);
        transform.LookAt(center + Vector3.up * lookHeight);
    }
}
