using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Менеджер уровня (один на сцену): HUD, рыба, портал, смерть и победа.
public class GameManager : MonoBehaviour
{
    public static GameManager I;     // другие скрипты пишут GameManager.I
    public static int totalFish;     // рыба за всю игру (static — не сбрасывается при смене сцены)

    public string nextScene = "Level2";   // куда идём после уровня
    public int fishNeeded = 5;            // сколько рыбы открывает портал (0 — уровень с боссом)
    public string startMessage = "";      // подсказка в начале уровня

    public Health player;
    public Portal portal;
    public Image[] hearts;     // 3 сердца в HUD
    public Text fishText;      // «3 / 5»
    public Text messageText;   // большая надпись по центру
    public AudioClip winClip;

    public bool Ending { get; private set; }   // уровень заканчивается (смерть или победа)

    int fish;              // рыба на этом уровне
    string sceneToLoad;    // какую сцену загрузить

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        ShowMessage(startMessage, 4f);
    }

    void Update()
    {
        // сердца: живые — красные, потерянные — тёмные
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].color = i < player.hp ? Color.red : new Color(0f, 0f, 0f, 0.4f);

        // счётчик рыбы
        if (fishNeeded > 0) fishText.text = fish + " / " + fishNeeded;
        else fishText.text = (totalFish + fish).ToString();
    }

    public void AddFish()
    {
        fish++;
        if (fish == fishNeeded)
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
        if (Ending) return;
        Ending = true;
        ShowMessage("OOPS! TRY AGAIN", 2f);
        sceneToLoad = SceneManager.GetActiveScene().name;
        Invoke(nameof(LoadScene), 1.2f);
    }

    // портал или победа над боссом: следующая сцена
    public void LevelComplete()
    {
        if (Ending) return;
        Ending = true;
        totalFish += fish;
        ShowMessage("LEVEL COMPLETE!", 2.5f);
        GetComponent<AudioSource>().PlayOneShot(winClip);
        sceneToLoad = nextScene;
        Invoke(nameof(LoadScene), 2.5f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
