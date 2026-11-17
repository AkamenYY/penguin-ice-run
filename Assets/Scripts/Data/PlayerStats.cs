using UnityEngine;

// ДАННЫЕ пингвина (ScriptableObject). Все числа баланса игрока живут в ассете Data/Player_Stats,
// их меняют в Inspector — код трогать не нужно.
[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float moveSpeed;        // скорость бега
    public float jumpSpeed;        // сила прыжка
    public int maxHp;              // жизни (сердца)
    public float invincibleTime;   // неуязвимость после удара, сек
    public float throwCooldown;    // перезарядка снежка, сек
    public float snowballSpeed;    // скорость снежка
}
