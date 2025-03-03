using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int playerCount = 0;
    private bool gameStarted = false;
    public TextMeshProUGUI p1JoinDialogue;
    public TextMeshProUGUI p2JoinDialogue;
    public TextMeshProUGUI p3JoinDialogue;
    private float startTime;
    public TextMeshProUGUI pressHold;
    public Image progBar;
    private float percentage = 0;
    private float decriment = .01f;

    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            p1JoinDialogue.text = "Player 1 Ready";
            playerCount++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            p2JoinDialogue.text = "Player 2 Ready";
            playerCount++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            p3JoinDialogue.text = "Player 3 Ready";
            playerCount++;
        }

        if(playerCount > 0)
        {
            pressHold.text = "Player 1 hold '2' to start " + playerCount.ToString() + " Player game";

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                startTime = Time.time;
                
            }


            if (percentage > 1)
            {
                // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Julian_Testing"));
                SceneManager.LoadSceneAsync("MainScene");
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                percentage = Time.time - startTime;
                progBar.fillAmount = percentage;

            } else if (percentage > 0)
            {
                percentage -= decriment;
                progBar.fillAmount = percentage;

            }


        }


    }
}
