using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class StartManager : MonoBehaviour
{
    public int playerCount = 0;
    public GameObject p1glow, p2glow, p3glow;
    public Image p1Ready, p2Ready, p3Ready;
    public Image eppee, teebee, heartly;
    //public TextMeshProUGUI pressHold;
    public GameObject timerTextObject;
    public GameObject wellWellWell;
    private TextMeshProUGUI timerText;
    private float startTime = 20;
    private SpriteAnimation ep, tb, hl;
    private SpriteAnimation p1, p2, p3;
    private bool[] joined;
    public string[] playerRoles = new string[3];
    private int[] playerIndices = new int[3];

    void Awake()
    {
        timerText = timerTextObject.GetComponent<TextMeshProUGUI>();
        
        wellWellWell.SetActive(false);
        ep = eppee.GetComponent<SpriteAnimation>();
        tb = teebee.GetComponent<SpriteAnimation>();
        hl = heartly.GetComponent<SpriteAnimation>();
        p1 = p1glow.GetComponent<SpriteAnimation>();
        p2 = p2glow.GetComponent<SpriteAnimation>();
        p3 = p3glow.GetComponent<SpriteAnimation>();
        p1.LoadSpritesFromFolder("podiumOff");
        p1.playAnimation();
        p2.LoadSpritesFromFolder("podiumOff2");
        p2.playAnimation();
        p3.LoadSpritesFromFolder("podiumOff");
        p3.playAnimation();

        joined = new bool[3];
        for (int i = 0; i < playerIndices.Length; i++)
        {
            playerIndices[i] = -1;
        }

        if(UnityEngine.Random.Range(0,1001) == 666)
        {
            wellWellWell.SetActive(true);
            wellWellWell.GetComponent<SpriteAnimation>().playAnimation();
            wellWellWell.GetComponent<AudioSource>().Play();
        }
    }

    void Update()
    {
        if (playerCount < 3)
        {
            if (Input.GetButtonDown("2") && !joined[0] || Input.GetButtonDown("1") && !joined[0] || Input.GetButtonDown("3") && !joined[0] || Input.GetButtonDown("4") && !joined[0])
            {
                joined[0] = true;
                AddRole("eppee", 0);
                p1Ready.enabled = false;
                p1.LoadSpritesFromFolder("podiumOn");
                p1.playAnimation();
                StartCoroutine(PlaySequentialAnimations(eppee, ep, "eppi_on state", "eppi_off", -650, -220));
            }
            else if (Input.GetButtonDown("con2") && !joined[1] || Input.GetButtonDown("con1") && !joined[1] || Input.GetButtonDown("con3") && !joined[1] || Input.GetButtonDown("con4") && !joined[1])
            {
                joined[1] = true;
                AddRole("teebee", 1);
                p2Ready.enabled = false;
                p2.LoadSpritesFromFolder("podiumOn");
                p2.playAnimation();
                StartCoroutine(PlaySequentialAnimations(teebee, tb, "tb_on state", "tb_idle", 22, -265));
            }
            else if (Input.GetButtonDown("uparrow") && !joined[2] || Input.GetButtonDown("leftarrow") && !joined[2] || Input.GetButtonDown("rightarrow") && !joined[2] || Input.GetButtonDown("downarrow") && !joined[2])
            {
                joined[2] = true;
                AddRole("heartly", 2);
                p3Ready.enabled = false;
                p3.LoadSpritesFromFolder("podiumOn");
                p3.playAnimation();
                StartCoroutine(PlaySequentialAnimations(heartly, hl, "heartly_on state", "heartly_idle", 660, -220));
            }
        }

        if (GameManager.Instance.playerCount > 0)
        {
            if(startTime < 0)
            {
                SceneManager.LoadSceneAsync("Onboarding");
            }
            //else if(GameManager.Instance.playerCount == 3)
            //{
            //    //take out later
            //    SceneManager.LoadSceneAsync("Onboarding");
            //}

            timerTextObject.SetActive(true);
            startTime -= Time.deltaTime;
            timerText.text = ":" + Mathf.Round(startTime).ToString();

            // Calculate opacity
            float t = 1 - (startTime / 20);
            float alpha = 0.01f + Mathf.Clamp01(t * t);

            // Set the color with new alpha
            Color color = timerText.color;
            color.a = alpha;
            timerText.color = color;
            //pressHold.text = "Player 1 press 'Circle' to start a " + GameManager.Instance.playerCount.ToString() + " Player game";

            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    SceneManager.LoadSceneAsync("Onboarding");
            //}
        }
    }

    private void AddRole(string role, int index)
    {
        playerRoles[playerCount] = role;
        playerCount++;

        string[] ordered = playerRoles
            .Where(r => !string.IsNullOrEmpty(r))
            .OrderBy(r => GetVisualIndex(r))
            .ToArray();

        GameManager.Instance.SetPlayerCount(playerCount, ordered);
    }

    private int GetVisualIndex(string role)
    {
        switch (role)
        {
            case "eppee": return 0;
            case "teebee": return 1;
            case "heartly": return 2;
            default: return 99;
        }
    }

    private IEnumerator PlaySequentialAnimations(Image char1, SpriteAnimation anim, string firstFolder, string secondFolder, int x, int y)
    {
        anim.LoadSpritesFromFolder(firstFolder);
        //char1.rectTransform.sizeDelta = new Vector2(403, 528);
        //char1.rectTransform.anchoredPosition = new Vector2(x, y);
        anim.loop = false;
        anim.playAnimation();

        while (anim.IsPlaying)
        {
            yield return null;
        }

        anim.LoadSpritesFromFolder(secondFolder);
        anim.loop = true;
        //if(char1 == eppee)
        //{
        //    char1.rectTransform.sizeDelta = new Vector2(450, 300);
        //}
        //else
        //{
        //    char1.rectTransform.sizeDelta = new Vector2(403, 360);
        //}

            //char1.rectTransform.anchoredPosition = new Vector2(x, y - 70);
        anim.playAnimation();
    }
}
