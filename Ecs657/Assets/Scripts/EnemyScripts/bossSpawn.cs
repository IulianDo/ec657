using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // The boss GameObject you want to spawn
    public GameObject bossCamera; // Reference to the boss camera
    public GameObject playerCamera; // Reference to the player camera
    public float bossSpawnTime = 300.0f; // 5 minutes in seconds
    [SerializeField] private Timer timer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject player;

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

            // Change the player layer to the enemy layer
            ChangeLayer(player, "Ground");

            // Spawn the boss GameObject
            SpawnBoss();

            // Delay for some time using Invoke
            float delayTime = 3f; // Adjust the time as needed
            Invoke("SwitchBackToPlayerLayer", delayTime);
        }
    }

    void SwitchCamera(GameObject cameraToActivate)
    {
        playerCamera.SetActive(false);
        cameraToActivate.SetActive(true);
    }

    void SwitchBackToPlayerLayer()
    {
        // Change the player layer back to the player layer
        ChangeLayer(player, "Player");

        // Switch back to the player camera
        SwitchCamera(playerCamera);
    }

    void ChangeLayer(GameObject obj, string newLayerName)
    {
        // Find the layer by name
        int newLayer = LayerMask.NameToLayer(newLayerName);

        // Check if the layer exists
        if (newLayer == -1)
        {
            Debug.LogError("Layer " + newLayerName + " not found!");
            return;
        }

        // Change the layer of the GameObject
        obj.layer = newLayer;

        // If the GameObject has child objects, change their layers recursively
        foreach (Transform child in obj.transform)
        {
            ChangeLayer(child.gameObject, newLayerName);
        }
    }

    void SpawnBoss()
    {
        // Spawn the boss GameObject
        Instantiate(bossPrefab, spawnPoint.transform.position, transform.rotation);
    }
}
