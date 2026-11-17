using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Менеджер уровня (один на сцену). Один и тот же код для всех уровней:
// всё, чем уровни отличаются (цель, время, подсказка, враги, следующий уровень), — в ассете LevelData.
public class GameManager : MonoBehaviour
{
    public static GameManager I;     // другие скрипты пишут GameManager.I
    public static int totalFish;     // рыба за всю игру (static — не сбрасывается при смене сцены)

    public LevelData level;    // ДАННЫЕ уровня: Level_01 … Level_04
    public ItemValues items;   // ДАННЫЕ предметов

    public Health player;
    public Portal portal;
    public Image[] hearts;     // сердца в HUD
    public Text fishText;      // «3 / 5»
    public Text levelText;     // «LEVEL 1   87» — название и оставшееся время
    public Text messageText;   // большая надпись по центру
    public AudioClip winClip;

    public bool Ending { get; private set; }   // уровень заканчивается (смерть или победа)

    int fish;              // рыба на этом уровне
    float timeLeft;        // секунд до конца времени
    string sceneToLoad;    // какую сцену загрузить

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        AudioListener.volume = SaveSystem.Data.volume;   // настройка из save.json
        timeLeft = level.timeLimit;
        ShowMessage(level.startMessage, 4f);

        // враги из данных уровня: префаб, точка, поворот, длина патруля
        foreach (EnemySpawn s in level.enemies)
        {
            GameObject e = Instantiate(s.prefab, s.position, Quaternion.Euler(0f, s.angle, 0f));
            e.GetComponent<Enemy>().patrolDistance = s.patrolDistance;
        }
    }

    void Update()
    {
        // сердца: живые — красные, потерянные — тёмные
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].color = i < player.hp ? Color.red : new Color(0f, 0f, 0f, 0.4f);

        // счётчик рыбы
        if (level.fishGoal > 0) fishText.text = fish + " / " + level.fishGoal;
        else fishText.text = (totalFish + fish).ToString();

        // таймер уровня
        if (!Ending)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f) Lose("TIME UP!");
        }
        if (levelText != null) levelText.text = level.title + "   " + Mathf.CeilToInt(Mathf.Max(timeLeft, 0f));
    }

    public void AddFish()
    {
        int before = fish;
        fish += items.fishPerPickup;
        if (before < level.fishGoal && fish >= level.fishGoal)
        {
            portal.Open();
            ShowMessage("THE PORTAL IS OPEN!", 2.5f);
        }
    }

    // надпись по центру на несколько секунд
    public void ShowMessage(string text, float seconds)
    {
        messageText.text = text;
        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), seconds);
    }

    void HideMessage()
    {
        messageText.text = "";
    }

    // смерть: этот же уровень заново
    public void Restart()
    {
        Lose("OOPS! TRY AGAIN");
    }

    void Lose(string message)
    {
        if (Ending) return;
        Ending = true;
        ShowMessage(message, 2f);
        sceneToLoad = SceneManager.GetActiveScene().name;
        Invoke(nameof(LoadScene), 1.2f);
    }

    // портал или победа над боссом: сохраняем прогресс и идём дальше
    public void LevelComplete()
    {
        if (Ending) return;
        Ending = true;
        totalFish += fish;
        SaveSystem.CompleteLevel(level.number, totalFish);   // СОХРАНЕНИЕ, момент 1: после уровня
        ShowMessage("LEVEL COMPLETE!", 2.5f);
        GetComponent<AudioSource>().PlayOneShot(winClip);
        sceneToLoad = level.nextScene;
        Invoke(nameof(LoadScene), 2.5f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // СОХРАНЕНИЕ, момент 3: игру свернули (Home, звонок). Может быть последним кодом, который успеет выполниться.
    void OnApplicationPause(bool paused)
    {
        if (paused) SaveSystem.Save();
    }
}
