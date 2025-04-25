using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class EndScript : MonoBehaviour
{
    public TextMeshProUGUI EndBanner;
    public TextMeshProUGUI[] pScores;
    public Image[] podiums;
    public Image[] glows;
    public Image[] chars;
    public GameObject[] playerPlaces;
    public VideoPlayer outro;
    public AudioSource fail;
    public GameObject simonFail;
    private int scoreTotal;


    private float outroTime = 10f;

    void Awake()
    {
        // Hide all podiums initially
        foreach (GameObject place in playerPlaces)
        {
            place.SetActive(false);
        }
        simonFail.SetActive(false);
        int playerCount = GameManager.Instance.playerCount;
        string[] roles = new string[playerCount];
        int[] scores = new int[playerCount];

        for (int i = 0; i < playerCount; i++)
        {
            roles[i] = GameManager.Instance.pRoles[i];
            scores[i] = GameManager.Instance.playerPoints[i, 1];
        }

        // Pair roles and scores for sorting
        var playerData = roles.Select((role, index) => new { Role = role, Score = scores[index], Index = index }).ToList();
        var sorted = playerData.OrderByDescending(p => p.Score).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            string role = sorted[i].Role;
            int score = sorted[i].Score;
            int podiumIndex = GetPodiumIndex(playerCount, i, role);
            if (podiumIndex >= 0 && podiumIndex < playerPlaces.Length)
            {
                GameObject podium = playerPlaces[podiumIndex];
                podium.SetActive(true);

                Transform scoreObj = podium.transform.Find("score");
                if (scoreObj != null && scoreObj.TryGetComponent(out TextMeshProUGUI scoreText))
                {
                    scoreText.text = "Score: " + score;
                }
            }
        }

        // Banner display
        EndBanner.text = GameManager.Instance.difficultyLevel == 2
            ? "Game Over"
            : "Round " + (GameManager.Instance.difficultyLevel + 1) + " Over";

        // Score list update (optional legacy UI)
        for (int i = 0; i < playerCount; i++)
        {
            pScores[i].text = scores[i] + " PTS";
            scoreTotal += scores[i];
        }

        if(scoreTotal <= 1500)
        {
            fail.Play();
            simonFail.SetActive(true);
            simonFail.GetComponent<SpriteAnimation>().playAnimation();
        }
    }

    private int GetPodiumIndex(int playerCount, int placement, string role)
    {
        int offset = 0;
        int roleIndex = RoleToIndex(role); // 0=eppee, 1=teebee, 2=heartly

        if (playerCount == 1)
        {
            return roleIndex; // indexes 0–2
        }
        else if (playerCount == 2)
        {
            offset = (placement == 0) ? 9 : 12;
        }
        else if (playerCount == 3)
        {
            offset = placement * 3;
        }

        return offset + roleIndex;
    }

    private int RoleToIndex(string role)
    {
        return role switch
        {
            "eppee" => 0,
            "teebee" => 1,
            "heartly" => 2,
            _ => -1
        };
    }

    void Update()
    {
        outroTime -= Time.deltaTime;
        if (outroTime <= 0f)
        {
            // Do something when outro is done
        }
    }
}
