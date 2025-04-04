using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;           
    public AudioClip originalClip;            
    public AudioClip testingSceneClip;        

    private static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);   
        }
        else
        {
            Destroy(gameObject);             
            return;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Set the original clip at the start
        if (audioSource.clip == null && originalClip != null)
        {
            audioSource.clip = originalClip;
            audioSource.Play();
        }

        // Subscribe to scene change event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Julian_Testing")
        {
            // Switch to Julian_Testing music
            if (audioSource.clip != testingSceneClip)
            {
                audioSource.clip = testingSceneClip;
                audioSource.Play();
            }
        }
        else if (scene.name == "RestScene")
        {
            // Switch back to the original music
            if (audioSource.clip != originalClip)
            {
                audioSource.clip = originalClip;
                audioSource.Play();
            }
        }
    }
}
