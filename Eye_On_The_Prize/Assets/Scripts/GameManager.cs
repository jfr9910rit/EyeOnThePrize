using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public int playerCount = 0;
    public GameObject[] playerSequences = new GameObject[6];
    public TextMeshProUGUI TimerText;
    public float gameTimer = 25f;
    public float hideTime = 10f;
    public bool isTimerRunning = false;
    public int playersFinished = 0;
    public int[,] playerPoints; //fix this
    public int difficultyLevel = 0;

    // referances for inputs used by podiums
    private Dictionary<string, string[]> podiumInputs = new Dictionary<string, string[]> {
        {"Podium_A", new string[] {"1", "2", "3", "4" } },
        {"Podium_B", new string[] {"con1", "con2", "con3", "con4"} },
        {"Podium_C", new string[] {"leftarrow", "uparrow", "rightarrow", "downarrow"} },
    };

    public Dictionary<string, string[]> playerInputs = new Dictionary<string, string[]>
    {
        {"Player_1", new string[] {} },
        {"Player_2", new string[] {} },
        {"Player_3", new string[] {} },
    };

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
                //canTakeInput = false;  // Stop user input
                //CheckSequence();  // Immediately check sequence
                SceneManager.LoadScene("EndScene");
            }
            TimerText.text = Mathf.RoundToInt(gameTimer).ToString();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignSceneVariables();
    }

    void AssignSceneVariables()
    {
        TimerText = GameObject.Find("Timer")?.GetComponent<TextMeshProUGUI>();
        playerSequences[0] = GameObject.Find("Sequence_L1_1P");
        playerSequences[1] = GameObject.Find("Sequence_L1_2P_P1");
        playerSequences[2] = GameObject.Find("Sequence_L1_2P_P2");
        playerSequences[3] = GameObject.Find("Sequence_L1_3P_P1");
        playerSequences[4] = GameObject.Find("Sequence_L1_3P_P2");
        playerSequences[5] = GameObject.Find("Sequence_L1_3P_P3");
        
        UpdateActiveSequences();
    }

    public void SetPlayerCount(int count, string p)
    {
        // set player inputs the podium that was registered.
        playerInputs["Player_"+count] = podiumInputs[p];

        playerCount = count;
        playerPoints = new int[playerCount, 2];
        //playersFinished = 0; // Reset player finish count
        difficultyLevel = 0;
        UpdateActiveSequences();
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
            difficultyLevel++;
            gameTimer = 25f - ((float)difficultyLevel * 5f);
            hideTime = 10f - ((float)difficultyLevel * 2.5f);
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
            if (playerSequences[0] != null) playerSequences[0].SetActive(true);
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
}
