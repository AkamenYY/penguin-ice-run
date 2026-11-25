using System.Collections.Generic;

// ДАННЫЕ ИГРОКА — то, что пишется в save.json. Только поля и List (JsonUtility не сохраняет Dictionary и свойства).
[System.Serializable]
public class SaveData
{
    public int version = 2;                                    // v2: вместо одной громкости — музыка и звуки
    public int bestScore;                                      // больше всего рыбы за одну игру
    public List<int> unlockedLevels = new List<int> { 1 };     // открытые уровни, 1-й открыт всегда
    public float musicVolume = 1f;                             // настройка: громкость музыки (0..1)
    public float sfxVolume = 1f;                               // настройка: громкость звуков (0..1)
}
