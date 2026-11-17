using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Главное меню и экран победы.
// PLAY → выбор уровня (закрытые уровни — LOCKED, берутся из save.json).
// SETTINGS → громкость и Reset progress с подтверждением.
public class MenuUI : MonoBehaviour
{
    public Text fishText;              // только на экране победы

    [Header("Выбор уровня")]
    public GameObject levelPanel;
    public LevelData[] levels;         // Level_01 … Level_04
    public Button[] levelButtons;      // кнопка на каждый уровень

    [Header("Настройки")]
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public GameObject confirmPanel;    // «Delete all progress?»
    public Text confirmText;

    void Start()
    {
        Time.timeScale = 1f;   // если вышли в меню с паузы — время снова идёт
        AudioListener.volume = SaveSystem.Data.volume;

        if (fishText != null)
            fishText.text = "FISH RESCUED: " + GameManager.totalFish + "\nBEST: " + SaveSystem.Data.bestScore;

        if (volumeSlider != null) volumeSlider.value = SaveSystem.Data.volume;
    }

    // ---------- выбор уровня ----------

    public void Play()
    {
        levelPanel.SetActive(true);
        for (int i = 0; i < levels.Length; i++)
        {
            bool open = SaveSystem.Data.unlockedLevels.Contains(levels[i].number);
            levelButtons[i].interactable = open;
            levelButtons[i].GetComponentInChildren<Text>().text = open ? levels[i].title : "LOCKED";
        }
    }

    public void LoadLevel(int index)
    {
        GameManager.totalFish = 0;   // новая игра — счёт с нуля
        SceneManager.LoadScene(levels[index].sceneName);
    }

    public void CloseLevels()
    {
        levelPanel.SetActive(false);
    }

    // ---------- настройки ----------

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void SetVolume(float value)   // слайдер
    {
        SaveSystem.Data.volume = value;
        AudioListener.volume = value;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        SaveSystem.Save();   // СОХРАНЕНИЕ, момент 2: настройки изменились
    }

    // ---------- сброс прогресса ----------

    public void AskReset()
    {
        SaveData d = SaveSystem.Data;
        confirmText.text = "Best score " + d.bestScore + " and " + d.unlockedLevels.Count + " unlocked levels will be deleted.";
        confirmPanel.SetActive(true);
    }

    public void CancelReset()
    {
        confirmPanel.SetActive(false);
    }

    public void ConfirmReset()
    {
        SaveSystem.ResetProgress();                                   // файл и память
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   // меню заново, с чистыми данными
    }

    // ---------- прочее ----------

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        SaveSystem.Save();
        Application.Quit();   // закрыть игру (в редакторе Unity ничего не делает)
    }

    // СОХРАНЕНИЕ, момент 3: игру свернули
    void OnApplicationPause(bool paused)
    {
        if (paused) SaveSystem.Save();
    }
}
