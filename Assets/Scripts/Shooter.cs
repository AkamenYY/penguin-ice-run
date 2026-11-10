using UnityEngine;

// ВРАГ 2 — снеговик-стрелок. Стоит на месте и стреляет ледяными шарами.
// Состояния: Watch (крутится, осматривается) → Aim (увидел: поворот к игроку, «!» — замах) → выстрел → Cooldown (отдых).
// Стена между снеговиком и игроком — луч не проходит, он не стреляет.
public class Shooter : Enemy
{
    enum State { Watch, Aim, Cooldown }

    [Header("Зрение")]
    public float viewRange = 12f;
    public float viewAngle = 90f;
    public float lookAroundSpeed = 40f;   // как быстро крутится в Watch (градусов в сек)
    public float aimTurnSpeed = 180f;     // как быстро поворачивается к игроку в Aim

    [Header("Атака")]
    public float windUp = 0.6f;           // прицеливание: «!» над головой
    public float cooldown = 1.6f;         // отдых после выстрела
    public float shotSpeed = 8f;
    public Projectile shotPrefab;         // ледяной шар (BossShot)
    public Transform shootPoint;
    public AudioClip shootClip;

    State state = State.Watch;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Watch:    Watch();    break;
            case State.Aim:      Aim();      break;
            case State.Cooldown: Cooldown(); break;
        }
    }

    void Watch()
    {
        transform.Rotate(0f, lookAroundSpeed * Time.deltaTime, 0f);   // осматривается

        if (CanSeePlayer(viewRange, viewAngle))
        {
            state = State.Aim;
            timer = 0f;
            ShowWarning(true);
        }
    }

    void Aim()
    {
        TurnToPlayer(aimTurnSpeed);

        // игрок спрятался за стену или убежал — отмена
        if (!CanSeePlayer(viewRange, 360f))
        {
            state = State.Watch;
            ShowWarning(false);
        }
        else if (timer >= windUp)
        {
            Shoot();
            state = State.Cooldown;
            timer = 0f;
            ShowWarning(false);
        }
    }

    void Cooldown()
    {
        if (timer >= cooldown) state = State.Watch;
    }

    void Shoot()
    {
        Vector3 target = player.position + Vector3.up * 0.5f;
        Projectile shot = Instantiate(shotPrefab, shootPoint.position, Quaternion.LookRotation(target - shootPoint.position));
        shot.speed = shotSpeed;   // Start() у шара ещё не вызван — успеваем задать скорость
        GetComponent<AudioSource>().PlayOneShot(shootClip);
    }
}
