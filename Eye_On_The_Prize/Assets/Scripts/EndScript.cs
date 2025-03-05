using TMPro;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI EndBanner;

    void Start()
    {
        Debug.Log("end scene");
        GameManager.Instance.isTimerRunning = false;
        Debug.Log(PlayerPrefs.GetInt($"Player{0}Score", 0));
        Debug.Log(GameManager.Instance.playerCount);
        for (int i = 1; i <= GameManager.Instance.playerCount; i++)
        {
            Debug.Log(PlayerPrefs.GetInt($"Player{i}Score", 0));
            EndBanner.text = EndBanner.text + "\nPlayer " + i + " score: " + PlayerPrefs.GetInt($"Player{i}Score", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
