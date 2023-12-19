using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // The boss GameObject you want to spawn
    public float bossSpawnTime = 300.0f; // 5 minutes in seconds
    [SerializeField] private Timer timer;
    [SerializeField] private Transform spawnPoint;

    public bool bossSpawned = false;

    void Start()
    {
        // Find the Timer component and assign it to the 'timer' variable
        timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    void Update()
    {
        // Check if the timer has reached the specified time and the boss has not been spawned yet
        if (timer != null && timer.timeValue <= bossSpawnTime && !bossSpawned)
        {
            // Set a flag to prevent spawning more than once
            bossSpawned = true;
            SpawnBoss();
            Destroy(gameObject);
        }
    }

    void SpawnBoss()
    {
        // Spawn the boss GameObject
        Instantiate(bossPrefab, spawnPoint.transform.position, transform.rotation);
    }
}