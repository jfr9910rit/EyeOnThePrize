using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartManager : MonoBehaviour
{
    public int playerCount = 0;
    public Image p1glow;
    public Image p2glow;
    public Image p3glow;
    public Image p1Ready;
    public Image p2Ready;
    public Image p3Ready;
    public Image eppee;
    public Image teebee;
    public Image heartly;
    public TextMeshProUGUI pressHold;

    private float startTime;
    private SpriteAnimation ep;
    private SpriteAnimation tb;
    private SpriteAnimation hl;
    private bool[] joined;

    // Public array to store joined player roles in order
    public string[] playerRoles = new string[3];
    private int[] playerIndices = new int[3]; // Maps roles to player slot (0-2)

    void Start()
    {
        
    }

    void Awake()
    {
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
                AssignPlayer("eppee", 0);
                p1Ready.enabled = false;
                p1glow.enabled = true;
                StartCoroutine(PlaySequentialAnimations(eppee, ep, "eppee_spawn", "eppee_resting", -650, -200));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) && !joined[1])
            {
                joined[1] = true;
                AssignPlayer("teebee", 1);
                p2Ready.enabled = false;
                p2glow.enabled = true;
                StartCoroutine(PlaySequentialAnimations(teebee, tb, "teebee_spawn", "teebee_resting", 22, -243));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !joined[2])
            {
                joined[2] = true;
                AssignPlayer("heartly", 2);
                p3Ready.enabled = false;
                p3glow.enabled = true;
                StartCoroutine(PlaySequentialAnimations(heartly, hl, "heartly_spawn", "heartly_resting", 660, -200));
            }
        }

        if (GameManager.Instance.playerCount > 0)
        {
            pressHold.text = "Player 1 hold 'Circle' to start " + GameManager.Instance.playerCount.ToString() + " Player game";

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneManager.LoadSceneAsync("Julian_Testing");
            }
        }
    }

    private void AssignPlayer(string role, int roleIndex)
    {
        playerRoles[playerCount] = role;
        playerIndices[roleIndex] = playerCount;
        playerCount++;
        GameManager.Instance.SetPlayerCount(GameManager.Instance.playerCount);
        GameManager.Instance.SetRoles(role);
        //for(int i = 0; i < playerRoles.Length; i++)
        //{
        //    Debug.Log(playerRoles[i]);
        //}
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
        char1.rectTransform.sizeDelta = new Vector2(403, 360);
        char1.rectTransform.anchoredPosition = new Vector2(x, y - 70);
        anim.playAnimation();
    }
}
