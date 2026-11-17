using UnityEngine;

// БАЗОВЫЙ класс всех врагов. Наследники: Seal, Patroller, Shooter, Charger, Boss.
// Общее: жизни, ссылка на игрока, «вижу ли игрока» (дистанция + угол + луч), красный «!» перед атакой.
// Все числа баланса — в ассете EnemyStats (поле stats), не в коде.
public abstract class Enemy : MonoBehaviour
{
    public EnemyStats stats;            // ДАННЫЕ: Seal_Stats / Patroller_Stats / Shooter_Stats / Charger_Stats
    public bool touchDamage = true;     // бьёт касанием (старый тюлень и босс). Новые враги — false, у них своя атака
    public GameObject warning;          // красный «!» над головой: показываем во время замаха (wind-up)
    public float patrolDistance;        // длина патруля — ставит GameManager из LevelData

    protected int hp;                   // текущие жизни — у КАЖДОГО врага свои (в ассете только maxHp)
    protected bool dead;
    protected Transform player;

    protected virtual void Start()
    {
        hp = MaxHp();
        GameObject p = GameObject.FindWithTag("Player");   // ищем один раз, а не каждый кадр
        if (p != null) player = p.transform;               // в главном меню игрока нет — тюлени просто ползают
        ShowWarning(false);
    }

    // сколько жизней у врага (босс берёт из своего BossStats — переопределяет)
    protected virtual int MaxHp()
    {
        return stats.maxHp;
    }

    // попал снежок
    public virtual void TakeHit()
    {
        if (dead) return;
        hp--;
        if (hp <= 0)
        {
            dead = true;
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    // ---------- общее для ИИ ----------

    // Видит ли враг игрока? 3 проверки: 1) близко  2) в угле обзора  3) луч до игрока не упёрся в стену
    protected bool CanSeePlayer(float range, float angle)
    {
        Vector3 eye = transform.position + Vector3.up * 0.6f;
        Vector3 toPlayer = player.position + Vector3.up * 0.6f - eye;

        if (toPlayer.magnitude > range) return false;                                // 1) далеко
        if (Vector3.Angle(transform.forward, toPlayer) > angle / 2f) return false;   // 2) вне угла обзора

        // 3) Raycast: первое, во что попал луч, — это игрок? (триггеры вроде рыбы не считаем)
        // AllLayers — потому что пингвин лежит на слое Ignore Raycast (так его проверка земли не находит саму себя)
        if (Physics.Raycast(eye, toPlayer, out RaycastHit hit, range, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            return hit.transform == player;
        return false;
    }

    protected float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    // плавно повернуться к игроку (только по горизонтали), скорость поворота — stats.turnSpeed
    protected void TurnToPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir == Vector3.zero) return;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), stats.turnSpeed * Time.deltaTime);
    }

    protected void ShowWarning(bool on)
    {
        if (warning != null) warning.SetActive(on);
    }

    protected void HitPlayer()
    {
        player.GetComponent<Health>().TakeDamage();
    }

    // касается пингвина — урон (только если touchDamage включён)
    void OnCollisionStay(Collision c)
    {
        if (!touchDamage || dead) return;
        Health health = c.collider.GetComponent<Health>();
        if (health != null) health.TakeDamage();
    }
}
