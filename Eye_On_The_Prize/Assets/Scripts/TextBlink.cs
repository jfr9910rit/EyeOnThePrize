using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TextBlink : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToUse;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOnStart = false;
    [SerializeField] private float timeMultiplier;
    private bool FadeIncomplete = false;

    private void Start()
    {

        StartCoroutine(IntroFade(textToUse));
    }

    private void Update()
    {
        if (Input.GetButtonDown("1") || Input.GetButtonDown("2") || Input.GetButtonDown("3") || Input.GetButtonDown("4") ||
        Input.GetButtonDown("con1") || Input.GetButtonDown("con2") || Input.GetButtonDown("con3") || Input.GetButtonDown("con4") ||
        Input.GetButtonDown("leftarrow") || Input.GetButtonDown("uparrow") || Input.GetButtonDown("rightarrow") || Input.GetButtonDown("downarrow"))
        {
            SceneManager.LoadSceneAsync("StartScene");
        }

    }



    private IEnumerator IntroFade(TextMeshProUGUI textToUse)
    {
        for(int i = 0; i <100; i++)
        {
            yield return StartCoroutine(FadeInText(timeMultiplier, textToUse));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(FadeOutText(timeMultiplier, textToUse));
        }
        
        //End of transition, do some extra stuff!!
    }

    private IEnumerator FadeInText(float timeSpeed, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }
    private IEnumerator FadeOutText(float timeSpeed, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }
    //public void FadeInText(float timeSpeed = -1.0f)
    //{
    //    if (timeSpeed <= 0.0f)
    //    {
    //        timeSpeed = timeMultiplier;
    //    }
    //    StartCoroutine(FadeInText(timeSpeed, textToUse));
    //}
    //public void FadeOutText(float timeSpeed = -1.0f)
    //{
    //    if (timeSpeed <= 0.0f)
    //    {
    //        timeSpeed = timeMultiplier;
    //    }
    //    StartCoroutine(FadeOutText(timeSpeed, textToUse));
    //}
}