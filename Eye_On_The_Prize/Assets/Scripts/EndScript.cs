using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;
using System.Reflection;



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
    public float moveSpeed = 600f; // units per second
    public float scoreSpeed = 1500f; // points per second
    public Image round;
    public AudioClip diss;
    public AudioClip college;

    //Class for moving the podium at the end
    private class MovingPlayer
    {
        public GameObject podium;
        public Vector3 startPos;
        public Vector3 targetPos;
        public TextMeshProUGUI scoreText;
        public int finalScore;
        public float displayedScore;
        public bool reachedTarget;
    }

    private List<MovingPlayer> movingPlayers = new List<MovingPlayer>();

    //plays on every time scene is pulled up
    void Awake()
    {
        //hide all 
        foreach (GameObject place in playerPlaces)
        {
            place.SetActive(false);
        }

        simonFail.SetActive(false);

        int playerCount = GameManager.Instance.playerCount;
        string[] roles = new string[playerCount];
        int[] scores = new int[playerCount];
        // Sorting the players by points and then making it so the correct options are up depedning on role and sorting by points
        for (int i = 0; i < playerCount; i++)
        {
            roles[i] = GameManager.Instance.pRoles[i];
            scores[i] = GameManager.Instance.playerPoints[i, 1];
        }

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
                    Vector3 originalPos = podium.transform.position;
                    Vector3 belowPos = originalPos - new Vector3(0f, 800f, 0f);
                    podium.transform.position = belowPos;

                    movingPlayers.Add(new MovingPlayer
                    {
                        podium = podium,
                        startPos = belowPos,
                        targetPos = originalPos,
                        scoreText = scoreText,
                        finalScore = score,
                        displayedScore = 0f,
                        reachedTarget = false
                    });

                    scoreText.text = "0 PTS";
                }
            }
        }
        
        string r = (GameManager.Instance.difficultyLevel + 1).ToString();
        round.GetComponent<SpriteAnimation>().LoadSpritesFromFolder(r);
        round.GetComponent<SpriteAnimation>().playAnimation();



        int scoreTotal = 0;
        for (int i = 0; i < playerCount; i++)
        {
            pScores[i].text = scores[i] + " PTS";
            scoreTotal += scores[i];
        }
        int rand = UnityEngine.Random.Range(0, 2);
        if(UnityEngine.Random.Range(0, 5) == 1)
        {
            if (rand == 0)
            {
                fail.clip = diss;
                fail.Play();
                //simonFail.SetActive(true);
                //simonFail.GetComponent<SpriteAnimation>().playAnimation();
            }
            else if (rand == 1)
            {
                fail.clip = college;
                fail.Play();
            }
        }
        
    }
    // move player and increase points over time
    void Update()
    {
        foreach (var moving in movingPlayers)
        {
            if (!moving.reachedTarget)
            {
                moving.podium.transform.position = Vector3.MoveTowards(moving.podium.transform.position, moving.targetPos, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(moving.podium.transform.position, moving.targetPos) < 0.1f)
                {
                    moving.podium.transform.position = moving.targetPos;
                    moving.reachedTarget = true;
                }
            }

            if (moving.displayedScore < moving.finalScore)
            {
                moving.displayedScore += scoreSpeed * Time.deltaTime;
                if (moving.displayedScore > moving.finalScore)
                    moving.displayedScore = moving.finalScore;

                moving.scoreText.text = Mathf.FloorToInt(moving.displayedScore) + " PTS";
            }
        }
    }
    // set proper podium index
    private int GetPodiumIndex(int playerCount, int placement, string role)
    {
        int offset = 0;
        int roleIndex = RoleToIndex(role);

        if (playerCount == 1)
        {
            return roleIndex;
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
    //int for roles
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
}
