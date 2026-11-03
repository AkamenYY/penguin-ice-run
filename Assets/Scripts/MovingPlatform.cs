using UnityEngine;

// Движущаяся льдина. Двигает её Animation Clip (A5) через Animator.
// Этот скрипт только считает скорость льдины, чтобы пингвин «ехал» вместе с ней (см. PenguinController).
public class MovingPlatform : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }

    Vector3 last;

    void Start()
    {
        last = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 v = (transform.position - last) / Time.fixedDeltaTime;
        v.y = 0f;   // по вертикали пингвина двигает сама физика
        Velocity = v;
        last = transform.position;
    }
}
