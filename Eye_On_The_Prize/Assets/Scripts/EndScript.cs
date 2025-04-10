using TMPro;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI EndBanner;
    //public TextMeshProUGUI p1Score;
    //public TextMeshProUGUI p2Score;
    //public TextMeshProUGUI p3Score;

    public TextMeshProUGUI[] pScores;

    void Awake()
    {
        Debug.Log("end scene");
        GameManager.Instance.isTimerRunning = false;
        //Debug.Log(PlayerPrefs.GetInt($"Player{0}Score", 0));
        //Debug.Log(GameManager.Instance.playerPoints[0,1]);
        //Debug.Log(GameManager.Instance.playerCount);
        //for (int i = 0; i < GameManager.Instance.playerCount; i++)
        //{
        //    //Debug.Log(PlayerPrefs.GetInt($"Player{i}Score", 0));
        //    Debug.Log(GameManager.Instance.playerPoints[i,1]);
        //    EndBanner.text = EndBanner.text + "\nPlayer " + (i + 1) + " score: " + GameManager.Instance.playerPoints[i, 1]/*PlayerPrefs.GetInt($"Player{i}Score", 0)*/;




        //}
        if (GameManager.Instance.difficultyLevel == 2)
        {
            EndBanner.text = "Game Over \nPress any button to Restart";
        }
        else {
            EndBanner.text = "Round " + (GameManager.Instance.difficultyLevel + 1) + " Over";
        }
        for(int i = 0; i < GameManager.Instance.playerCount; i++)
        {
            pScores[i].text = "Player " + (i +1) +  "\nScore: " + GameManager.Instance.playerPoints[i, 1];
        }
        //p1Score.text = "Player 1 \nScore: " + GameManager.Instance.playerPoints[0, 1];
        //p2Score.text = "Player 2 \nScore: " + GameManager.Instance.playerPoints[1, 1];
        //p3Score.text = "Player 3 \nScore: " + GameManager.Instance.playerPoints[2, 1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
