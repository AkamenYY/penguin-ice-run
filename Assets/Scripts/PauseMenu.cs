using UnityEngine;
using UnityEngine.SceneManagement;

// Пауза: кнопка «II» в HUD открывает панель, время останавливается (Time.timeScale = 0).
// На панели: Resume (продолжить), Restart (уровень заново), Menu (в главное меню).
public class PauseMenu : MonoBehaviour
{
    public GameObject panel;   // тёмная панель паузы с кнопками

    void Start()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;   // на всякий случай: уровень всегда начинается не на паузе
    }

    // кнопка «II» в HUD
    public void Pause()
    {
        panel.SetActive(true);
        GameManager.I.messageText.text = "";   // убрать подсказку, чтобы не налезала на PAUSED
        Time.timeScale = 0f;   // время стоит: физика, корутины с WaitForSeconds, анимации
    }

    // кнопка RESUME
    public void Resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;   // время идёт снова
    }

    // кнопка RESTART
    public void Restart()
    {
        Time.timeScale = 1f;   // сначала вернуть время, иначе новая сцена тоже будет «замороженной»
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // кнопка MENU
    public void ToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
