using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;           
    public AudioClip originalClip;            
    public AudioClip testingSceneClip;   
    public AudioClip gong;
    public AudioClip applause;

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
            audioSource.loop = true;
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
    //depedning on what scene is loaded play certain sounds such as bg music and also applause and round end sound
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Julian_Testing")
        {
            // Switch to Julian_Testing music
            if (audioSource.clip != testingSceneClip)
            {
                audioSource.clip = testingSceneClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (scene.name == "RestScene")
        {
            // Switch back to the original music
            if (audioSource.clip != originalClip)
            {
                audioSource.clip = originalClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (scene.name == "Onboarding")
        {
            audioSource.Stop();
        }
        else if(scene.name == "EndScene")
        {
            if (audioSource.clip != gong)
            {
                audioSource.clip = gong;
                audioSource.loop = false;
                audioSource.Play();
            }
        }
        else if(scene.name == "Outro")
        {
            if (audioSource.clip != applause)
            {
                audioSource.clip = applause;
                audioSource.loop = false;
                audioSource.Play();
            }
        }
    }
}
