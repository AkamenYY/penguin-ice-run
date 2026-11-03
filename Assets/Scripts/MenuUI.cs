using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Кнопки главного меню и экрана победы.
public class MenuUI : MonoBehaviour
{
    public Text fishText;   // только на экране победы

    void Start()
    {
        Time.timeScale = 1f;   // если вышли в меню с паузы — время снова идёт
        if (fishText != null) fishText.text = "FISH RESCUED: " + GameManager.totalFish;
    }

    public void Play()
    {
        GameManager.totalFish = 0;          // новая игра — счёт с нуля
        SceneManager.LoadScene("Level1");
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();   // закрыть игру (в редакторе Unity ничего не делает)
    }
}
