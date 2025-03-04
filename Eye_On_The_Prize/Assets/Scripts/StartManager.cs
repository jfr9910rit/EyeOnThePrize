using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int playerCount = 0;
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
        //change logic so it can be 2 players with slots 2 and 3 being full being p1 and p2 and single player being any poistion
        if (Input.GetKeyDown(KeyCode.Alpha1))//make all buttons
        {
            p1JoinDialogue.text = "Player 1 Ready";
            GameManager.Instance.SetPlayerCount(GameManager.Instance.playerCount + 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))//make all buttons
        {
            p2JoinDialogue.text = "Player 2 Ready";
            GameManager.Instance.SetPlayerCount(GameManager.Instance.playerCount + 1); 
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))//make all buttons
        {
            p3JoinDialogue.text = "Player 3 Ready";
            GameManager.Instance.SetPlayerCount(GameManager.Instance.playerCount + 1);
        }
        //add timer
        if(GameManager.Instance.playerCount > 0)
        {
            pressHold.text = "Player 1 hold 'Square' to start " + GameManager.Instance.playerCount.ToString() + " Player game";

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                startTime = Time.time;
                
            }


            if (percentage > 1)
            {
                // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Julian_Testing"));
                SceneManager.LoadSceneAsync("Julian_Testing");
            }
            else if (Input.GetKey(KeyCode.Alpha2))//change key
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
