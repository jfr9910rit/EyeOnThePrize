using System.Diagnostics;
using System.Reflection;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI EndBanner;
    

    public TextMeshProUGUI[] pScores;
    public Image[] podiums;
    public Image[] glows;
    public Image[] chars;
    public VideoPlayer outro;
    private float outroTime;

    void Awake()
    {
        outroTime = 10f;
        for(int i = 0; i < GameManager.Instance.playerCount; i++)
        {
            //podiums[i].setActive(false);
            //glows[i].setActive(false);
            //chars[i].setActive(false);
            //pScores[i].setActive(false);
        }


        //GameManager.Instance.isTimerRunning = false;

        //if (GameManager.Instance.playerCount == 1)
        //{
        //    if (GameManager.Instance.pRoles[0] == "eppee")
        //    {
                
        //    }
        //    else if (GameManager.Instance.pRoles[0] == "teebee")
        //    {
                
        //    }
        //    else if (GameManager.Instance.pRoles[0] == "heartly")
        //    {
                
        //    }

        //}
        //else if (GameManager.Instance.playerCount == 2)
        //{
        //    if (playerInt == 0)
        //    {
        //        if (GameManager.Instance.pRoles[0] == "eppee")
        //        {
                    
        //        }
        //        else if (GameManager.Instance.pRoles[0] == "teebee")
        //        {
                    
        //        }
        //    }
        //    else if (playerInt == 1)
        //    {
        //        if (GameManager.Instance.pRoles[1] == "teebee")
        //        {
                    
        //        }
        //        else if (GameManager.Instance.pRoles[1] == "heartly")
        //        {
                    
        //        }
        //    }
        //}
        //else if (GameManager.Instance.playerCount == 3)
        //{

        //}




        if (GameManager.Instance.difficultyLevel == 2)
        {
            EndBanner.text = "Game Over";

        }
        else {
            EndBanner.text = "Round " + (GameManager.Instance.difficultyLevel + 1) + " Over";
        }
        for(int i = 0; i < GameManager.Instance.playerCount; i++)
        {
            pScores[i].text = /*"Player " + (i +1) +  */"Score: " + GameManager.Instance.playerPoints[i, 1];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        outroTime -= Time.deltaTime;
        if(outroTime == 0)
        {
            //Debug.Log("Done!");
        }
    }
}
