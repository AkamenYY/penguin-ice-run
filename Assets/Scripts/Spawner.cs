using System.Collections;
using UnityEngine;

// Спавнер падающих сосулек (Level2): как часто — в LevelData, скорость и время метки — в ItemValues.
// Сначала на льду появляется красная метка, потом сверху падает сосулька — игрок успевает отойти.
public class Spawner : MonoBehaviour
{
    public Projectile prefab;          // сосулька
    public GameObject warningPrefab;   // красная метка
    public Vector3 area;               // размер зоны (раскладка сцены)
    public float height;               // с какой высоты падает

    IEnumerator Start()   // Start может быть корутиной
    {
        float interval = GameManager.I.level.icicleInterval;
        ItemValues items = GameManager.I.items;
        if (interval <= 0f) yield break;   // в этом уровне сосулек нет

        while (true)
        {
            yield return new WaitForSeconds(interval);
            Vector3 p = transform.position + new Vector3(Random.Range(-area.x, area.x) / 2f, 0f, Random.Range(-area.z, area.z) / 2f);
            Destroy(Instantiate(warningPrefab, p, Quaternion.identity), items.icicleWarnTime + 0.5f);
            yield return new WaitForSeconds(items.icicleWarnTime);
            Projectile icicle = Instantiate(prefab, p + Vector3.up * height, Quaternion.identity);
            icicle.speed = items.icicleSpeed;
        }
    }
}
