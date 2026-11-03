using UnityEngine;

// Панель SafeArea: каждый кадр подгоняет себя под «безопасную зону» экрана —
// без выреза камеры (чёлки) и закруглённых углов. Все кнопки и тексты лежат внутри неё,
// поэтому на телефоне с вырезом ничего не уходит под чёлку.
[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    RectTransform panel;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
    }

    void Update()
    {
        Rect safe = Screen.safeArea;   // безопасная зона в пикселях

        // переводим пиксели в якоря от 0 до 1 (доля экрана)
        Vector2 min = safe.position;
        Vector2 max = safe.position + safe.size;
        min.x /= Screen.width;
        min.y /= Screen.height;
        max.x /= Screen.width;
        max.y /= Screen.height;

        panel.anchorMin = min;   // левый нижний угол панели
        panel.anchorMax = max;   // правый верхний угол панели
        panel.offsetMin = Vector2.zero;
        panel.offsetMax = Vector2.zero;
    }
}
