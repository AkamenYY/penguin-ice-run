using UnityEngine;
using UnityEngine.AI;

// ВРАГ 3 — морж-таран. Медленно идёт к игроку по NavMesh, потом делает быстрый рывок по прямой.
// Состояния: Idle (идёт к игроку) → WindUp (стоит, «!», трясётся) → Charge (рывок) → Rest (устал, можно бить).
public class Charger : Enemy
{
    enum State { Idle, WindUp, Charge, Rest }

    [Header("Движение")]
    public float walkSpeed = 1.5f;
    public float noticeRange = 12f;     // с какого расстояния начинает идти к игроку

    [Header("Зрение")]
    public float chargeRange = 7f;      // с какого расстояния делает рывок
    public float viewAngle = 60f;

    [Header("Атака")]
    public float windUp = 0.7f;         // замах: «!» и тряска
    public float chargeSpeed = 11f;
    public float chargeTime = 0.7f;     // длина рывка в секундах
    public float restTime = 1.5f;       // отдых после рывка (кулдаун)

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
        agent.speed = walkSpeed;
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
        if (DistanceToPlayer() < noticeRange) agent.SetDestination(player.position);

        if (DistanceToPlayer() < chargeRange && CanSeePlayer(chargeRange, viewAngle))
        {
            state = State.WindUp;
            timer = 0f;
            agent.ResetPath();   // остановился
            ShowWarning(true);
        }
    }

    void WindUp()
    {
        TurnToPlayer(240f);
        model.localPosition = new Vector3(Mathf.Sin(timer * 60f) * 0.06f, 0f, 0f);   // тряска — видно, что сейчас рванёт

        if (timer >= windUp)
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
        agent.Move(chargeDir * chargeSpeed * Time.deltaTime);   // Move не даёт выехать за NavMesh и в стены

        if (!hitDone && DistanceToPlayer() < 1.3f)
        {
            hitDone = true;
            HitPlayer();
        }

        if (timer >= chargeTime)
        {
            state = State.Rest;
            timer = 0f;
        }
    }

    void Rest()
    {
        if (timer >= restTime) state = State.Idle;
    }
}
