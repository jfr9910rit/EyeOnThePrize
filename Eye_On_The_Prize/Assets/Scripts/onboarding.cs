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
    private bool p1Skip, p2Skip, p3Skip;
    public GameObject[] playerSkips = new GameObject[3];
    public Sprite skipped, unskipped;

    void Awake()
    {
        anim = animation.GetComponent<SpriteAnimation>();
        StartCoroutine(RunSequence());
        p1Skip = false;
        p2Skip = false;
        p3Skip = false;
        playerSkips[0].SetActive(false);
        playerSkips[1].SetActive(false);
        playerSkips[2].SetActive(false);
        playerSkips[0].GetComponent<Image>().sprite = unskipped;
        playerSkips[1].GetComponent<Image>().sprite = unskipped;
        playerSkips[2].GetComponent<Image>().sprite = unskipped;

        for (int i = 0; i < GameManager.Instance.playerCount; i++)
        {
            playerSkips[i].SetActive(true);
        }
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

    void Update()
    {
        if (GameManager.Instance.playerCount == 1 && p1Skip == true)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else if (GameManager.Instance.playerCount == 2 && p1Skip == true && p2Skip == true)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else if (GameManager.Instance.playerCount == 3 && p1Skip == true && p2Skip == true && p3Skip == true)
        {
            SceneManager.LoadScene(nextSceneName);
        }

        //ROLES
        if (Input.GetButtonDown("4") && p1Skip == false)
        {
            playerSkips[0].GetComponent<Image>().sprite = skipped;
            p1Skip = true;
        }
        else if (Input.GetButtonDown("con4") && p2Skip == false)
        {
            playerSkips[1].GetComponent<Image>().sprite = skipped;
            p2Skip = true;
        }
        else if (Input.GetButtonDown("downarrow") && p3Skip == false)
        {
            playerSkips[2].GetComponent<Image>().sprite = skipped;
            p3Skip = true;
        }
    }
}
