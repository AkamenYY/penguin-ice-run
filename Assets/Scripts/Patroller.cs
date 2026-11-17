using UnityEngine;
using UnityEngine.AI;

// ВРАГ 1 — тюлень-охранник. Ходит по NavMesh.
// Состояния: Patrol (ходит туда-обратно) → Chase (увидел — бежит за игроком) → Attack (замах, укус, отдых).
// Потерял игрока из виду на stats.loseTime секунд → снова Patrol. Все числа — в Patroller_Stats.
public class Patroller : Enemy
{
    enum State { Patrol, Chase, Attack }

    State state = State.Patrol;
    NavMeshAgent agent;
    Vector3[] points;   // 2 точки патруля: где появился и patrolDistance вперёд
    int point;          // номер текущей точки патруля
    float timer;        // таймер текущего состояния
    bool bitten;        // уже укусил в этой атаке

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        points = new[] { transform.position, transform.position + transform.forward * patrolDistance };
        GoPatrol();
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase:  Chase();  break;
            case State.Attack: Attack(); break;
        }
    }

    void Patrol()
    {
        // дошёл до точки — идём к следующей
        if (!agent.pathPending && agent.remainingDistance < 0.3f)
        {
            point = (point + 1) % points.Length;
            agent.SetDestination(points[point]);
        }

        if (CanSeePlayer(stats.viewRange, stats.viewAngle)) GoChase();
    }

    void Chase()
    {
        agent.SetDestination(player.position);

        if (CanSeePlayer(stats.viewRange, stats.viewAngle)) timer = 0f;   // видим — сбрасываем таймер «потерял»

        if (DistanceToPlayer() < stats.attackRange) GoAttack();
        else if (timer > stats.loseTime) GoPatrol();
    }

    void Attack()
    {
        TurnToPlayer();

        // конец замаха — кусаем, если игрок всё ещё рядом (успел отбежать — промах)
        if (!bitten && timer >= stats.windUp)
        {
            bitten = true;
            ShowWarning(false);
            if (DistanceToPlayer() < stats.hitRange) HitPlayer();
        }

        if (timer >= stats.windUp + stats.cooldown) GoChase();
    }

    // ---------- переходы между состояниями ----------

    void GoPatrol()
    {
        state = State.Patrol;
        agent.speed = stats.moveSpeed;
        agent.SetDestination(points[point]);
    }

    void GoChase()
    {
        state = State.Chase;
        timer = 0f;
        agent.isStopped = false;
        agent.speed = stats.chaseSpeed;
    }

    void GoAttack()
    {
        state = State.Attack;
        timer = 0f;
        bitten = false;
        agent.isStopped = true;   // во время замаха стоит на месте
        ShowWarning(true);
    }
}
