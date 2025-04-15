using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;


public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public int playerCount = 0;
    public GameObject[] playerSequences = new GameObject[10];
    public string[] pRoles = new string[3];
    public TextMeshProUGUI TimerText;
    public float gameTimer = 25f;
    public float hideTime = 10f;
    public bool isTimerRunning = false;
    public int playersFinished = 0;
    public int[,] playerPoints; //fix this
    public int difficultyLevel = 0;
    public string activeModifier = "None";
    public Image glowRing;

    void Awake()
    {
        Debug.Log(playerCount);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         //initialize players
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning && SceneManager.GetActiveScene().name == "Julian_Testing")
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                gameTimer = 0;
                isTimerRunning = false;
                SceneManager.LoadScene("EndScene");
            }

            TimerText.text = Mathf.RoundToInt(gameTimer).ToString();
            UpdateTimerColor(); // <-- add this line
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignSceneVariables();
        
    }

    void AssignSceneVariables()
    {
        TimerText = GameObject.Find("Timer")?.GetComponent<TextMeshProUGUI>();

        playerSequences[0] = GameObject.Find("Sequence_1P_EP");
        playerSequences[1] = GameObject.Find("Sequence_1P_TB");
        playerSequences[2] = GameObject.Find("Sequence_1P_HL");
        playerSequences[3] = GameObject.Find("Sequence_2P_P1_EP");
        playerSequences[4] = GameObject.Find("Sequence_2P_P1_TB");
        playerSequences[5] = GameObject.Find("Sequence_2P_P2_TB");
        playerSequences[6] = GameObject.Find("Sequence_2P_P3_HL");
        playerSequences[7] = GameObject.Find("Sequence_3P_P1");
        playerSequences[8] = GameObject.Find("Sequence_3P_P2");
        playerSequences[9] = GameObject.Find("Sequence_3P_P3");
        GameObject imageObject = GameObject.Find("glowring");
        glowRing = imageObject.GetComponent<Image>();
        glowRing.color = Color.green;
        UpdateActiveSequences();
    }

    public void SetPlayerCount(int count)
    {
        playerCount = count;
        playerPoints = new int[playerCount, 2];
        //playersFinished = 0; // Reset player finish count
        difficultyLevel = 0;
        UpdateActiveSequences();
    }

    public void SetRoles(string roles)
    {
        if(roles == eppee && playerCount == 1)
        {
            pRoles[0] = roles;
        }
        else if(roles == eppee && playerCount == 1)
    }

    public void StartGameTimer(int time)
    {
        gameTimer = time;
        isTimerRunning = true;
        //SequenceStarted = true;
    }

    public void PlayerFinished(int playerIndex, int score)
    {
        if (playerIndex >= 0 && playerIndex < playerPoints.Length)
        {
            playerPoints[playerIndex,1] += score;
            Debug.Log("test1");
        }
        playersFinished++;
        if (playersFinished >= playerCount)
        {
            isTimerRunning = false;
            Debug.Log(playerPoints[playerIndex, 1]);
            
            SceneManager.LoadScene("EndScene"); // change to end scene here
            
            //add other difficulty stuff here
        }
    }


    private void UpdateActiveSequences()
    {
        for (int i = 0; i < playerSequences.Length; i++)
        {
            if (playerSequences[i] != null)
            {
                playerSequences[i].SetActive(false);
            }
        }

        if (playerCount == 1)
        {
            if (pRoles == "eppee")
            {
                if (playerSequences[0] != null) playerSequences[0].SetActive(true);
            }
            else if(pRoles == "teebee")
            {

            }
            else if(pRoles == "heartly")
            {

            }
            
        }
        else if (playerCount == 2)
        {
            if (playerSequences[1] != null) playerSequences[1].SetActive(true);
            if (playerSequences[2] != null) playerSequences[2].SetActive(true);
        }
        else if (playerCount == 3)
        {
            if (playerSequences[3] != null) playerSequences[3].SetActive(true);
            if (playerSequences[4] != null) playerSequences[4].SetActive(true);
            if (playerSequences[5] != null) playerSequences[5].SetActive(true);
        }
    }

    private void UpdateTimerColor()
    {
        float t = 1f - (gameTimer / (25f - ((float)difficultyLevel * 5f))); // normalize from 0 (start) to 1 (end)

        if (t < 1f / 3f) // Green to Yellow
        {
            float lerpT = t * 3f;
            glowRing.color = Color.Lerp(Color.green, Color.yellow, lerpT);
        }
        else if (t < 2f / 3f) // Yellow to Red
        {
            float lerpT = (t - 1f / 3f) * 3f;
            glowRing.color = Color.Lerp(Color.yellow, Color.red, lerpT);
        }
        else // Hold Red
        {
            glowRing.color = Color.red;
        }
    }

}
