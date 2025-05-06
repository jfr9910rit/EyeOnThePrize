using UnityEngine;
using UnityEngine.SceneManagement;

public class TranstionTimer2 : MonoBehaviour
{

    public float gameTimer = 25f;
    public string nextScene = "";
    //same as other transition timer but will change where it goes to next to minimize scenes being made
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (GameManager.Instance.difficultyLevel == 1)
        {
            nextScene = "Round2";
        }
        else if (GameManager.Instance.difficultyLevel == 2)
        {
            nextScene = "Round3";
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
