using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    public GameObject circlePrefab;   // assign your circle prefab in inspector
    public float spawnInterval = 1.5f; // time between spawns
    public Vector2 spawnAreaMin = new Vector2(-5f, -3f);
    public Vector2 spawnAreaMax = new Vector2(5f, 3f);

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCircle();
            timer = 0f;
        }
    }

    void SpawnCircle()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        Vector2 spawnPos = new Vector2(x, y);

        Instantiate(circlePrefab, spawnPos, Quaternion.identity);
    }
}