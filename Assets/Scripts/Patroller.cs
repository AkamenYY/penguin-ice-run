using UnityEngine;
using UnityEngine.AI;

// ВРАГ 1 — тюлень-охранник. Ходит по NavMesh.
// Состояния: Patrol (ходит по точкам) → Chase (увидел — бежит за игроком) → Attack (замах, укус, отдых).
// Потерял игрока из виду на loseTime секунд → снова Patrol.
public class Patroller : Enemy
{
    enum State { Patrol, Chase, Attack }

    [Header("Движение")]
    public Transform[] points;          // точки патруля
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Зрение")]
    public float viewRange = 8f;
    public float viewAngle = 120f;
    public float loseTime = 2f;         // сколько секунд ищет, когда потерял игрока

    [Header("Атака")]
    public float attackRange = 1.6f;
    public float windUp = 0.5f;         // замах: «!» над головой, тюлень стоит
    public float cooldown = 1.2f;       // отдых после укуса

    State state = State.Patrol;
    NavMeshAgent agent;
    int point;          // номер текущей точки патруля
    float timer;        // таймер текущего состояния
    bool bitten;        // уже укусил в этой атаке

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
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
            agent.SetDestination(points[point].position);
        }

        if (CanSeePlayer(viewRange, viewAngle)) GoChase();
    }

    void Chase()
    {
        agent.SetDestination(player.position);

        if (CanSeePlayer(viewRange, viewAngle)) timer = 0f;   // видим — сбрасываем таймер «потерял»

        if (DistanceToPlayer() < attackRange) GoAttack();
        else if (timer > loseTime) GoPatrol();
    }

    void Attack()
    {
        TurnToPlayer(360f);

        // конец замаха — кусаем, если игрок всё ещё рядом (успел отбежать — промах)
        if (!bitten && timer >= windUp)
        {
            bitten = true;
            ShowWarning(false);
            if (DistanceToPlayer() < attackRange + 0.4f) HitPlayer();
        }

        if (timer >= windUp + cooldown) GoChase();
    }

    // ---------- переходы между состояниями ----------

    void GoPatrol()
    {
        state = State.Patrol;
        agent.speed = patrolSpeed;
        agent.SetDestination(points[point].position);
    }

    void GoChase()
    {
        state = State.Chase;
        timer = 0f;
        agent.isStopped = false;
        agent.speed = chaseSpeed;
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
