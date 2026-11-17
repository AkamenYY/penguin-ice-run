using UnityEngine;

// ДАННЫЕ врага (ScriptableObject). Один ассет на тип врага: Seal_Stats, Patroller_Stats, Shooter_Stats, Charger_Stats.
// Скрипт один, числа разные. Текущие жизни (hp) — НЕ здесь, а на каждом враге (Enemy.hp).
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Game/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Жизни и движение")]
    public int maxHp;            // сколько снежков выдерживает
    public float moveSpeed;      // обычный шаг / патруль
    public float chaseSpeed;     // погоня (тюлень)
    public float turnSpeed;      // поворот к игроку, градусов в сек
    public float scanSpeed;      // снеговик крутится, осматриваясь

    [Header("Зрение")]
    public float viewRange;      // дальность зрения, м
    public float viewAngle;      // угол обзора, градусов
    public float loseTime;       // сколько секунд ищет потерянного игрока

    [Header("Атака")]
    public float attackRange;    // с какого расстояния начинает атаку
    public float hitRange;       // докуда достаёт удар
    public float windUp;         // замах с «!», сек
    public float cooldown;       // отдых после атаки, сек
    public float shotSpeed;      // скорость ледяного шара (снеговик)
    public float chargeSpeed;    // скорость рывка (морж)
    public float chargeTime;     // длина рывка, сек
}
