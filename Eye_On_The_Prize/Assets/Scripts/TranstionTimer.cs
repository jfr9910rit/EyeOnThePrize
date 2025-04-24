using UnityEngine;
using UnityEngine.SceneManagement;

public class TranstionTimer : MonoBehaviour
{

    public float gameTimer = 25f;
    public string nextScene = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
