using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetToRest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetButtonDown("1") || Input.GetButtonDown("2") || Input.GetButtonDown("3") || Input.GetButtonDown("4") ||
        Input.GetButtonDown("con1") || Input.GetButtonDown("con2") || Input.GetButtonDown("con3") || Input.GetButtonDown("con4") ||
        Input.GetButtonDown("leftarrow") || Input.GetButtonDown("uparrow") || Input.GetButtonDown("rightarrow") || Input.GetButtonDown("downarrow"))
        {
            if(GameManager.Instance.difficultyLevel == 3)
            {
                GameManager.Instance.playerCount = 0;
                GameManager.Instance.difficultyLevel = 0;
                GameManager.Instance.gameTimer = 25f;
                GameManager.Instance.playersFinished = 0;
                GameManager.Instance.hideTime = 10f;
                SceneManager.LoadSceneAsync("RestScene");
            }
            else
            {
                GameManager.Instance.playersFinished = 0;
                SceneManager.LoadSceneAsync("Julian_Testing");
            }
            
        }
    }
}
