using System.Collections.Generic;
using UnityEngine;

// Один враг в уровне: какой префаб, где появляется, куда смотрит.
[System.Serializable]
public class EnemySpawn
{
    public GameObject prefab;       // Seal / Patroller / Shooter / Charger
    public Vector3 position;        // точка появления
    public float angle;             // куда смотрит, градусов по Y
    public float patrolDistance;    // длина патруля вперёд (тюлени), 0 — стоит
}

// ДАННЫЕ уровня (ScriptableObject): ассеты Level_01 … Level_04.
// Один и тот же GameManager читает любой из них: цель, время, подсказку, следующий уровень и врагов.
[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int number;              // номер уровня: для сохранений и выбора уровня
    public string title;            // «LEVEL 1» — в HUD и в меню
    public string sceneName;        // сцена с льдинами этого уровня
    public string nextScene;        // куда идём после победы
    public int fishGoal;            // рыба, открывающая портал (0 — уровень с боссом)
    public float timeLimit;         // секунд на уровень
    public string startMessage;     // подсказка в начале
    public float icicleInterval;    // сосульки раз в N секунд (0 — нет сосулек)
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
}
