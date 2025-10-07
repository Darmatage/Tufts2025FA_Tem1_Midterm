using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CircleSpawner : MonoBehaviour
{
    public GameObject circlePrefab;
    public float spawnInterval; // time between spawns
    public float circleLifetime; // how long a circle can be clicked

    public BattleHandlerScript battleHandler;

    public Vector2 spawnAreaMin = new Vector2(-5f, -3f);
    public Vector2 spawnAreaMax = new Vector2(5f, 3f);

    private float timer;
    private List<CircleData> circles = new List<CircleData>();
    private int nextCircleIndex = 0;

    private int circlesToSpawn = 0;
    private int circlesHit = 0;
    private bool spawningActive = false;

    void Update()
    {

        if (!spawningActive) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnCircle();
            timer = 0f;
        }

        // Auto-remove expired circles
        for (int i = 0; i < circles.Count; i++)
        {
            if (!circles[i].hit && Time.time - circles[i].spawnTime >= circleLifetime)
            {
                circles[i].circle.CircleMissed();
                circles[i].hit = true;

                // Move to next circle if the missed one was next
                if (i == nextCircleIndex)
                    nextCircleIndex++;
            }
        }

        if (nextCircleIndex >= circlesToSpawn)
        {
            spawningActive = false;
            StartCoroutine(Wait(circleLifetime));
        }

    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        battleHandler.attackFinished(circlesHit);
    }


    void SpawnCircle()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        Vector3 spawnPos = new Vector3(x, y, -nextCircleIndex * 0.01f);
        GameObject newCircle = Instantiate(circlePrefab, spawnPos, Quaternion.identity);
        CircleScript circleScript = newCircle.GetComponent<CircleScript>();
        circleScript.spawner = this;
        circleScript.SetLabel((circles.Count + 1).ToString());

        circles.Add(new CircleData { circle = circleScript, spawnTime = Time.time, hit = false });
    }

    public bool TryHit(CircleScript circle)
    {
        if (nextCircleIndex < circles.Count && circles[nextCircleIndex].circle == circle)
        {
            circles[nextCircleIndex].hit = true;
            nextCircleIndex++;
            circlesHit++;
            return true;
        }
        return false;
    }

    public void StartSpawning(int numCircles, float spawntime, float circletime)
    {
        foreach (var data in circles)
        {
            if (data.circle != null)
                Destroy(data.circle.gameObject);
        }

        circles.Clear();
        nextCircleIndex = 0;
        circlesHit = 0;
        circlesToSpawn = numCircles;
        spawnInterval = spawntime;
        circleLifetime = circletime;
        timer = 0f;
        spawningActive = true;
    }

    // Data structure to track each circle
    private class CircleData
    {
        public CircleScript circle;
        public float spawnTime;
        public bool hit;
    }
}