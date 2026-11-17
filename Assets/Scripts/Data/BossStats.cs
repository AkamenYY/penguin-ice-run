using UnityEngine;

// ДАННЫЕ босса (ScriptableObject): у Короля моржей свои атаки, поэтому свой ассет Data/Boss_Stats.
[CreateAssetMenu(fileName = "BossStats", menuName = "Game/Boss Stats")]
public class BossStats : ScriptableObject
{
    public int maxHp;
    public float startDelay;       // пауза перед первой атакой
    public float warnTime;         // «!» перед веером шаров
    public float pause;            // отдых между атаками (время бить босса)
    public float pausePhase2;      // отдых во 2-й фазе (меньше половины жизней)
    public int fanShots;           // шаров в веере
    public int fanShotsPhase2;
    public float fanSpread;        // ширина веера, градусов
    public float fanSpreadPhase2;
    public float shotSpeed;        // скорость шаров
    public int icicles;            // сосулек в ледяном дожде
    public int iciclesPhase2;
    public float rainRadius;       // разброс сосулек вокруг игрока, м
    public float rainWarnTime;     // метки на льду до падения, сек
}
