using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetToRest : MonoBehaviour
{

    public float timer = 10f;
    private bool done;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 10f;
        done = false;
    }

    // Update is called once per frame
    public void Update()
    {
        timer -= Time.deltaTime;
        if (Input.GetButtonDown("1") || Input.GetButtonDown("2") || Input.GetButtonDown("3") || Input.GetButtonDown("4") ||
        Input.GetButtonDown("con1") || Input.GetButtonDown("con2") || Input.GetButtonDown("con3") || Input.GetButtonDown("con4") ||
        Input.GetButtonDown("leftarrow") || Input.GetButtonDown("uparrow") || Input.GetButtonDown("rightarrow") || Input.GetButtonDown("downarrow") || timer <= 0 && !done)
        {
            if(GameManager.Instance.difficultyLevel == 2)
            {
                done = true;
                //Thomas Here
                GameManager.Instance.playerCount = 0;
                GameManager.Instance.difficultyLevel = 0;
                GameManager.Instance.gameTimer = 25f;
                GameManager.Instance.playersFinished = 0;
                GameManager.Instance.hideTime = 10f;
                SceneManager.LoadSceneAsync("Outro");
                
            }
            else
            {
                Debug.Log("increase");
                done = true;
                GameManager.Instance.playersFinished = 0;
                GameManager.Instance.difficultyLevel++;
                GameManager.Instance.gameTimer = 25f - ((float)GameManager.Instance.difficultyLevel * 5f);
                GameManager.Instance.hideTime = 10f - ((float)GameManager.Instance.difficultyLevel * 2.5f);
                SceneManager.LoadSceneAsync("postTrans");
            }
            
        }
    }
}
