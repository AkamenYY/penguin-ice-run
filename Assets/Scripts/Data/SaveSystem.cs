using System.IO;
using UnityEngine;

// СОХРАНЕНИЯ: save.json в Application.persistentDataPath (на телефоне — папка игры, переживает перезапуск).
// Data грузится один раз при первом обращении и живёт в памяти, пока игра открыта.
public static class SaveSystem
{
    static SaveData data;
    static string FilePath => Path.Combine(Application.persistentDataPath, "save.json");
    static string TempPath => Path.Combine(Application.persistentDataPath, "save.tmp");

    // В редакторе (Enter Play Mode без перезагрузки домена) static-поля живут между запусками Play.
    // Сбрасываем при каждом старте игры, чтобы save.json читался заново — как на телефоне.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetOnStart()
    {
        data = null;
    }

    public static SaveData Data
    {
        get
        {
            if (data == null) data = Load();
            return data;
        }
    }

    // Безопасная загрузка: нет файла или он сломан → значения по умолчанию, игра не падает
    static SaveData Load()
    {
        if (!File.Exists(FilePath)) return new SaveData();   // первый запуск или после сброса

        try
        {
            SaveData loaded = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
            return loaded ?? new SaveData();                  // пустой файл → null → по умолчанию
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Broken save, using defaults: " + e.Message);
            return new SaveData();
        }
    }

    // Запись: сначала полная копия в save.tmp, потом подменяем save.json.
    // Если батарея сядет во время записи, сломается только save.tmp.
    public static void Save()
    {
        File.WriteAllText(TempPath, JsonUtility.ToJson(Data, true));
        if (File.Exists(FilePath)) File.Replace(TempPath, FilePath, null);   // подмена одним шагом
        else File.Move(TempPath, FilePath);                                  // самое первое сохранение
    }

    // Пройден уровень: лучший счёт, открываем следующий уровень, сохраняем (момент 1)
    public static void CompleteLevel(int number, int score)
    {
        Data.bestScore = Mathf.Max(Data.bestScore, score);
        if (!Data.unlockedLevels.Contains(number + 1)) Data.unlockedLevels.Add(number + 1);
        Save();
    }

    // Сброс прогресса: удалить файл И очистить память (иначе следующее сохранение вернёт старое)
    public static void ResetProgress()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
        data = new SaveData();
    }
}
