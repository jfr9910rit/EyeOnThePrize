using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class StartManager : MonoBehaviour
{
    public int playerCount = 0;
    public Image p1glow, p2glow, p3glow;
    public Image p1Ready, p2Ready, p3Ready;
    public Image eppee, teebee, heartly;
    //public TextMeshProUGUI pressHold;
    public GameObject timerTextObject;

    private TextMeshProUGUI timerText;
    private float startTime = 20;
    private SpriteAnimation ep, tb, hl;
    private bool[] joined;
    public string[] playerRoles = new string[3];
    private int[] playerIndices = new int[3];

    void Awake()
    {
        timerText = timerTextObject.GetComponent<TextMeshProUGUI>();


        p1glow.enabled = false;
        p2glow.enabled = false;
        p3glow.enabled = false;

        ep = eppee.GetComponent<SpriteAnimation>();
        tb = teebee.GetComponent<SpriteAnimation>();
        hl = heartly.GetComponent<SpriteAnimation>();

        joined = new bool[3];
        for (int i = 0; i < playerIndices.Length; i++)
        {
            playerIndices[i] = -1;
        }
    }

    void Update()
    {
        if (playerCount < 3)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2) && !joined[0])
            {
                joined[0] = true;
                AddRole("eppee", 0);
                p1Ready.enabled = false;
                p1glow.enabled = true;
                StartCoroutine(PlaySequentialAnimations(eppee, ep, "eppee_spawn", "eppee_resting", -650, -220));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) && !joined[1])
            {
                joined[1] = true;
                AddRole("teebee", 1);
                p2Ready.enabled = false;
                p2glow.enabled = true;
                StartCoroutine(PlaySequentialAnimations(teebee, tb, "teebee_spawn", "teebee_resting", 22, -265));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !joined[2])
            {
                joined[2] = true;
                AddRole("heartly", 2);
                p3Ready.enabled = false;
                p3glow.enabled = true;
                StartCoroutine(PlaySequentialAnimations(heartly, hl, "heartly_spawn", "heartly_resting", 660, -220));
            }
        }

        if (GameManager.Instance.playerCount > 0)
        {
            if(startTime < 0)
            {
                SceneManager.LoadSceneAsync("Onboarding");
            }
            else if(GameManager.Instance.playerCount == 3)
            {
                SceneManager.LoadSceneAsync("Onboarding");
            }

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
        char1.rectTransform.sizeDelta = new Vector2(403, 528);
        char1.rectTransform.anchoredPosition = new Vector2(x, y);
        anim.loop = false;
        anim.playAnimation();

        while (anim.IsPlaying)
        {
            yield return null;
        }

        anim.LoadSpritesFromFolder(secondFolder);
        anim.loop = true;
        if(char1 == eppee)
        {
            char1.rectTransform.sizeDelta = new Vector2(450, 300);
        }
        else
        {
            char1.rectTransform.sizeDelta = new Vector2(403, 360);
        }

            char1.rectTransform.anchoredPosition = new Vector2(x, y - 70);
        anim.playAnimation();
    }
}
