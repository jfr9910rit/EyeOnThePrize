using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public int[,] playerPoints;
    public int difficultyLevel = 0;
    public string activeModifier = "None";
    public Image glowRing;

    void Awake()
    {
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

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

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
            UpdateTimerColor();
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
        playerSequences[6] = GameObject.Find("Sequence_2P_P2_HL");
        playerSequences[7] = GameObject.Find("Sequence_3P_P1");
        playerSequences[8] = GameObject.Find("Sequence_3P_P2");
        playerSequences[9] = GameObject.Find("Sequence_3P_P3");

        GameObject imageObject = GameObject.Find("glowring");
        glowRing = imageObject?.GetComponent<Image>();
        if (glowRing != null) glowRing.color = Color.green;

        UpdateActiveSequences();
    }

    public void SetPlayerCount(int count, string[] roles)
    {
        playerCount = count;
        playerPoints = new int[playerCount, 2];
        difficultyLevel = 0;

        for (int i = 0; i < roles.Length; i++)
        {
            pRoles[i] = roles[i];
            Debug.Log("Assigned pRole[" + i + "] = " + pRoles[i]);
        }

        UpdateActiveSequences();
    }

    public void StartGameTimer(int time)
    {
        gameTimer = time;
        isTimerRunning = true;
    }

    public void PlayerFinished(int playerIndex, int score)
    {
        if (playerIndex >= 0 && playerIndex < playerPoints.GetLength(0))
        {
            playerPoints[playerIndex, 1] += score;
        }

        playersFinished++;
        if (playersFinished >= playerCount)
        {
            isTimerRunning = false;
            SceneManager.LoadScene("EndScene");
        }
    }

    private void UpdateActiveSequences()
    {
        foreach (var seq in playerSequences)
        {
            if (seq != null) seq.SetActive(false);
        }

        if (playerCount == 1)
        {
            if (pRoles[0] == "eppee" && playerSequences[0] != null) playerSequences[0].SetActive(true);
            else if (pRoles[0] == "teebee" && playerSequences[1] != null) playerSequences[1].SetActive(true);
            else if (pRoles[0] == "heartly" && playerSequences[2] != null) playerSequences[2].SetActive(true);
        }
        else if (playerCount == 2)
        {
            if (pRoles[0] == "eppee" && playerSequences[3] != null) playerSequences[3].SetActive(true);
            else if (pRoles[0] == "teebee" && playerSequences[4] != null) playerSequences[4].SetActive(true);

            if (pRoles[1] == "teebee" && playerSequences[5] != null) playerSequences[5].SetActive(true);
            else if (pRoles[1] == "heartly" && playerSequences[6] != null) playerSequences[6].SetActive(true);
        }
        else if (playerCount == 3)
        {
            if (playerSequences[7] != null) playerSequences[7].SetActive(true);
            if (playerSequences[8] != null) playerSequences[8].SetActive(true);
            if (playerSequences[9] != null) playerSequences[9].SetActive(true);
        }
    }

    private void UpdateTimerColor()
    {
        float t = 1f - (gameTimer / (25f - (float)difficultyLevel * 5f));

        if (t < 1f / 3f)
        {
            glowRing.color = Color.Lerp(Color.green, Color.yellow, t * 3f);
        }
        else if (t < 2f / 3f)
        {
            glowRing.color = Color.Lerp(Color.yellow, Color.red, (t - 1f / 3f) * 3f);
        }
        else
        {
            glowRing.color = Color.red;
        }
    }
}