using System.Collections.Generic;

// ДАННЫЕ ИГРОКА — то, что пишется в save.json. Только поля и List (JsonUtility не сохраняет Dictionary и свойства).
[System.Serializable]
public class SaveData
{
    public int version = 1;                                    // поменяем формат — поднимем номер
    public int bestScore;                                      // больше всего рыбы за одну игру
    public List<int> unlockedLevels = new List<int> { 1 };     // открытые уровни, 1-й открыт всегда
    public float volume = 1f;                                  // настройка: громкость
}
