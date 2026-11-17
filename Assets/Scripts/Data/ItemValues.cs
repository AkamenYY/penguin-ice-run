using UnityEngine;

// ДАННЫЕ предметов и ловушек (ScriptableObject): ассет Data/Item_Values.
[CreateAssetMenu(fileName = "ItemValues", menuName = "Game/Item Values")]
public class ItemValues : ScriptableObject
{
    public int fishPerPickup;      // сколько рыбы даёт одна рыбка
    public int heartHeal;          // сколько жизней даёт сердце
    public float icicleSpeed;      // скорость падения сосульки
    public float icicleWarnTime;   // красная метка до падения, сек
}
