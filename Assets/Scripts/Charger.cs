using UnityEngine;
using UnityEngine.AI;

// ВРАГ 3 — морж-таран. Медленно идёт к игроку по NavMesh, потом делает быстрый рывок по прямой.
// Состояния: Idle (идёт к игроку) → WindUp (стоит, «!», трясётся) → Charge (рывок) → Rest (устал, можно бить).
// Все числа — в Charger_Stats: viewRange = когда замечает, attackRange = с какого расстояния рывок.
public class Charger : Enemy
{
    enum State { Idle, WindUp, Charge, Rest }

    State state = State.Idle;
    NavMeshAgent agent;
    Transform model;    // модель трясём во время замаха
    Vector3 chargeDir;  // направление рывка — фиксируется в конце замаха
    float timer;
    bool hitDone;       // уже ударил в этом рывке

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats.moveSpeed;
        model = transform.Find("Model");
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:   Idle();   break;
            case State.WindUp: WindUp(); break;
            case State.Charge: Charge(); break;
            case State.Rest:   Rest();   break;
        }
    }

    void Idle()
    {
        if (DistanceToPlayer() < stats.viewRange) agent.SetDestination(player.position);

        if (DistanceToPlayer() < stats.attackRange && CanSeePlayer(stats.attackRange, stats.viewAngle))
        {
            state = State.WindUp;
            timer = 0f;
            agent.ResetPath();   // остановился
            ShowWarning(true);
        }
    }

    void WindUp()
    {
        TurnToPlayer();
        model.localPosition = new Vector3(Mathf.Sin(timer * 60f) * 0.06f, 0f, 0f);   // тряска — видно, что сейчас рванёт

        if (timer >= stats.windUp)
        {
            model.localPosition = Vector3.zero;
            ShowWarning(false);
            chargeDir = transform.forward;
            state = State.Charge;
            timer = 0f;
            hitDone = false;
        }
    }

    void Charge()
    {
        agent.Move(chargeDir * stats.chargeSpeed * Time.deltaTime);   // Move не даёт выехать за NavMesh и в стены

        if (!hitDone && DistanceToPlayer() < stats.hitRange)
        {
            hitDone = true;
            HitPlayer();
        }

        if (timer >= stats.chargeTime)
        {
            state = State.Rest;
            timer = 0f;
        }
    }

    void Rest()
    {
        if (timer >= stats.cooldown) state = State.Idle;
    }
}
