using UnityEngine;

// ВРАГ 2 — снеговик-стрелок. Стоит на месте и стреляет ледяными шарами.
// Состояния: Watch (крутится, осматривается) → Aim (увидел: поворот к игроку, «!» — замах) → выстрел → Cooldown (отдых).
// Стена между снеговиком и игроком — луч не проходит, он не стреляет. Все числа — в Shooter_Stats.
public class Shooter : Enemy
{
    enum State { Watch, Aim, Cooldown }

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
        transform.Rotate(0f, stats.scanSpeed * Time.deltaTime, 0f);   // осматривается

        if (CanSeePlayer(stats.viewRange, stats.viewAngle))
        {
            state = State.Aim;
            timer = 0f;
            ShowWarning(true);
        }
    }

    void Aim()
    {
        TurnToPlayer();

        // игрок спрятался за стену или убежал — отмена
        if (!CanSeePlayer(stats.viewRange, 360f))
        {
            state = State.Watch;
            ShowWarning(false);
        }
        else if (timer >= stats.windUp)
        {
            Shoot();
            state = State.Cooldown;
            timer = 0f;
            ShowWarning(false);
        }
    }

    void Cooldown()
    {
        if (timer >= stats.cooldown) state = State.Watch;
    }

    void Shoot()
    {
        Vector3 target = player.position + Vector3.up * 0.5f;
        Projectile shot = Instantiate(shotPrefab, shootPoint.position, Quaternion.LookRotation(target - shootPoint.position));
        shot.speed = stats.shotSpeed;   // Start() у шара ещё не вызван — успеваем задать скорость
        GetComponent<AudioSource>().PlayOneShot(shootClip);
    }
}
