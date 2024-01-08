using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudio : MonoBehaviour
{
    private static PersistentAudio instance;
    private AudioSource audioSource;

    // Drag and drop the corresponding AudioClip assets in the Unity Editor
    public AudioClip level1AudioClip;
    public AudioClip level2AudioClip;
    public AudioClip level3AudioClip;

    void Start()
    {
        // Ensure only one instance of the persistent audio source exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Make the GameObject persist across level loads
        DontDestroyOnLoad(gameObject);

        // Add an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();

        // Play the corresponding audio clip based on the current level
        PlayLevelAudio();
    }

    void PlayLevelAudio()
    {
        // Get the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Stop the current audio source before changing the audio clip
        audioSource.Stop();

        // Assign the corresponding audio clip based on the scene name
        switch (currentSceneName)
        {
            case "level1":
                audioSource.clip = level1AudioClip;
                break;
            case "level2":
                audioSource.clip = level2AudioClip;
                break;
            case "level3":
                audioSource.clip = level3AudioClip;
                break;
            default:
                Debug.LogWarning("No corresponding audio clip found for the current level.");
                break;
        }

        // Check if an audio clip is loaded and play it
        if (audioSource.clip != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    // Called when a new level is loaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the corresponding audio clip when a new level is loaded
        PlayLevelAudio();
    }

    // Called when the script is disabled
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
