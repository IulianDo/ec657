using System.Collections;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // The boss GameObject you want to spawn
    public GameObject bossCamera; // Reference to the boss camera
    public GameObject playerCamera; // Reference to the player camera
    public float bossSpawnTime = 300.0f; // 5 minutes in seconds
    [SerializeField] private Timer timer;
    [SerializeField] private Transform spawnPoint;

    private bool bossSpawned = false;

    void Start()
    {
        // Find the Timer component and assign it to the 'timer' variable
        timer = GameObject.Find("Timer").GetComponent<Timer>();

        // Ensure the initial camera state is correct
        SwitchCamera(playerCamera);
    }

    void Update()
    {
        // Check if the timer has reached the specified time and the boss has not been spawned yet
        if (timer != null && timer.totalTimePassed >= bossSpawnTime && !bossSpawned)
        {
            // Set a flag to prevent spawning more than once
            bossSpawned = true;

            // Switch to the boss camera
            SwitchCamera(bossCamera);

            // Spawn the boss GameObject
            SpawnBoss();
            

            // Delay for some time and switch back to the player camera
            StartCoroutine(SwitchBackToPlayerCamera(3f)); // Adjust the time as needed
            
            //Destroy(gameObject);
        }
    }

    void SwitchCamera(GameObject cameraToActivate)
    {
        playerCamera.SetActive(false);
        cameraToActivate.SetActive(true);
    }

    IEnumerator SwitchBackToPlayerCamera(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        // Switch back to the player camera
        SwitchCamera(playerCamera);
    }

    void SpawnBoss()
    {
        // Spawn the boss GameObject
        Instantiate(bossPrefab, spawnPoint.transform.position, transform.rotation);
    }
}
