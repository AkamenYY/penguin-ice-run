using UnityEngine;

// Выход из уровня. Закрыт, пока не собрано нужное количество рыбы (тогда GameManager вызывает Open).
public class Portal : MonoBehaviour
{
    public GameObject closedVisual;   // тёмная заслонка
    public GameObject openVisual;     // светящийся диск
    public AudioClip openClip;

    bool open;

    void Start()
    {
        closedVisual.SetActive(true);
        openVisual.SetActive(false);
    }

    public void Open()
    {
        open = true;
        closedVisual.SetActive(false);
        openVisual.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(openClip);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (open) GameManager.I.LevelComplete();
        else GameManager.I.ShowMessage("COLLECT MORE FISH!", 2f);
    }
}
