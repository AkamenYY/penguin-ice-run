using UnityEngine;

// Тюлень (наследник Enemy): ползает туда-сюда по прямой на patrolDistance вперёд. Один снежок — и он исчезает.
// Скорость и жизни — в ассете Seal_Stats.
public class Seal : Enemy
{
    Vector3 pointA, pointB;
    Vector3 target;   // куда ползём сейчас

    protected override void Start()
    {
        base.Start();   // жизни — из Enemy
        pointA = transform.position;
        pointB = pointA + transform.forward * patrolDistance;
        target = pointB;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, stats.moveSpeed * Time.fixedDeltaTime);
        transform.LookAt(target);

        if (transform.position == target)   // дошли — разворот
            target = (target == pointB) ? pointA : pointB;
    }
}
