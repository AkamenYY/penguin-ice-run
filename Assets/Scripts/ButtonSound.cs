using UnityEngine;
using UnityEngine.UI;

// Висит на каждой кнопке UI: нажали — щелчок (AudioManager.Click).
[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(AudioManager.Click);
    }
}
