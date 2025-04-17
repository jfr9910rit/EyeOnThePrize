using TMPro;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI EndBanner;
    

    public TextMeshProUGUI[] pScores;

    void Awake()
    {
        Debug.Log("end scene");
        GameManager.Instance.isTimerRunning = false;
        




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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
