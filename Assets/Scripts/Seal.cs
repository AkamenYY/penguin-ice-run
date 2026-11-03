using UnityEngine;

// Тюлень (наследник Enemy): ползает туда-сюда между точками A и B. Один снежок — и он исчезает.
public class Seal : Enemy
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 2f;

    Vector3 target;   // куда ползём сейчас

    protected override void Start()
    {
        base.Start();   // жизни — из Enemy
        target = pointB;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        transform.LookAt(target);

        if (transform.position == target)   // дошли — разворот
            target = (target == pointB) ? pointA : pointB;
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
