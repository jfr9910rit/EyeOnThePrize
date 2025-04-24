using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class Onboarding : MonoBehaviour
{
    public Image animation; // Assign the GameObject with SpriteAnimation
    public string fadeOutFolder = "fade_out"; // Folder inside Resources
    public VideoPlayer videoPlayer; // Assign the VideoPlayer
    public string nextSceneName = "MainScene"; // Name of the next scene
    private SpriteAnimation anim;

    void Awake()
    {
        anim = animation.GetComponent<SpriteAnimation>();
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        // Start intro animation
        //introAnimation.playAnimation();
        yield return new WaitUntil(() => anim.HasFinished());

        // Play video
        videoPlayer.Play();
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        // Load fade-out sprites
        anim.LoadSpritesFromFolder(fadeOutFolder);
        anim.playAnimation(); // play fade out
        yield return new WaitUntil(() => anim.HasFinished());

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
