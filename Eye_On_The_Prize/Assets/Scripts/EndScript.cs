using TMPro;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI EndBanner;

    void Awake()
    {
        Debug.Log("end scene");
        GameManager.Instance.isTimerRunning = false;
        //Debug.Log(PlayerPrefs.GetInt($"Player{0}Score", 0));
        //Debug.Log(GameManager.Instance.playerPoints[0,1]);
        //Debug.Log(GameManager.Instance.playerCount);
        for (int i = 0; i < GameManager.Instance.playerCount; i++)
        {
            //Debug.Log(PlayerPrefs.GetInt($"Player{i}Score", 0));
            Debug.Log(GameManager.Instance.playerPoints[i,1]);
            EndBanner.text = EndBanner.text + "\nPlayer " + (i + 1) + " score: " + GameManager.Instance.playerPoints[i, 1]/*PlayerPrefs.GetInt($"Player{i}Score", 0)*/;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
