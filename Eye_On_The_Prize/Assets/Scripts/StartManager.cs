using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;


public class StartManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int playerCount = 0;
    //public TextMeshProUGUI p1JoinDialogue;
    //public TextMeshProUGUI p2JoinDialogue;
    //public TextMeshProUGUI p3JoinDialogue;
    public Image p1glow;
    public Image p2glow;
    public Image p3glow;
    public Image p1Ready;
    public Image p2Ready;
    public Image p3Ready;
    public Image eppee;
    public Image teebee;
    public Image heartly;
    private float startTime;
    public TextMeshProUGUI pressHold;
    public Image progBar;
    private float percentage = 0;
    private float decriment = .01f;
    private SpriteAnimation ep;
    private SpriteAnimation tb;
    private SpriteAnimation hl;

    void Start()
    {
        p1glow.enabled = false;
        p2glow.enabled = false;
        p3glow.enabled = false;
        p3glow.enabled = false;
    }

    void Awake()
    {
        ep = eppee.GetComponent<SpriteAnimation>();
        tb = teebee.GetComponent<SpriteAnimation>();
        hl = heartly.GetComponent<SpriteAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        //change logic so it can be 2 players with slots 2 and 3 being full being p1 and p2 and single player being any poistion
        if(playerCount < 3)
        {
            if (playerCount < 1 && Input.GetKeyDown(KeyCode.Alpha2))//make all buttons
            {
                //p1JoinDialogue.text = "Player 1 Ready";
                p1glow.enabled = true;
                p1Ready.enabled = false;
                GameManager.Instance.SetPlayerCount(GameManager.Instance.playerCount + 1);
                playerCount += 1;
                ep.LoadSpritesFromFolder("eppee_spawn");
                ep.playAnimation();
                ep.LoadSpritesFromFolder("eppee_resting");
                ep.playAnimation();
            }
            if (playerCount < 2 && Input.GetKeyDown(KeyCode.Alpha7))//make all buttons
            {
                //p2JoinDialogue.text = "Player 2 Ready";
                p2glow.enabled = true;
                p2Ready.enabled = false;
                GameManager.Instance.SetPlayerCount(GameManager.Instance.playerCount + 1);
                playerCount += 1;
                tb.LoadSpritesFromFolder("teebee_spawn");
                tb.playAnimation();

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))//make all buttons
            {
                //p3JoinDialogue.text = "Player 3 Ready";
                p3glow.enabled = true;
                p3Ready.enabled = false;
                GameManager.Instance.SetPlayerCount(GameManager.Instance.playerCount + 1);
                playerCount += 1;
                hl.LoadSpritesFromFolder("heartly_spawn");
                hl.playAnimation();
            }
        }

        //add timer
        if(GameManager.Instance.playerCount > 0)
        {
            pressHold.text = "Player 1 hold 'Circle' to start " + GameManager.Instance.playerCount.ToString() + " Player game";

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneManager.LoadSceneAsync("Julian_Testing");

            }


            //if (percentage > 1)
            //{
            //    // SceneManager.SetActiveScene(SceneManager.GetSceneByName("Julian_Testing"));
            //    SceneManager.LoadSceneAsync("Julian_Testing");
            //}
            //else if (Input.GetKey(KeyCode.Alpha1))//change key
            //{
            //    percentage = Time.time - startTime;
            //    progBar.fillAmount = percentage;

            //} else if (percentage > 0)
            //{
            //    percentage -= decriment;
            //    progBar.fillAmount = percentage;

            //}


        }


    }

    IEnumerator spawnAndIdle()
    {
        //going to make spawn and transition in idle here to make it so it can wait for first to finish easier
    }
}
