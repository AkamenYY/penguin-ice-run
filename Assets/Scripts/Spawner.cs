using System.Collections;
using UnityEngine;

// Спавнер падающих сосулек (Instantiate, A6): раз в interval секунд — случайная точка в зоне area.
// Сначала на льду появляется красная метка, через 0.9 сек сверху падает сосулька — игрок успевает отойти.
public class Spawner : MonoBehaviour
{
    public GameObject prefab;          // сосулька
    public GameObject warningPrefab;   // красная метка
    public Vector3 area = new Vector3(6f, 0f, 6f);
    public float interval = 2f;
    public float height = 7f;

    IEnumerator Start()   // Start может быть корутиной
    {
        yield return new WaitForSeconds(1.5f);
        while (true)
        {
            Vector3 p = transform.position + new Vector3(Random.Range(-area.x, area.x) / 2f, 0f, Random.Range(-area.z, area.z) / 2f);
            Destroy(Instantiate(warningPrefab, p, Quaternion.identity), 1.4f);
            yield return new WaitForSeconds(0.9f);
            Instantiate(prefab, p + Vector3.up * height, Quaternion.identity);
            yield return new WaitForSeconds(interval - 0.9f);
        }
    }
}
