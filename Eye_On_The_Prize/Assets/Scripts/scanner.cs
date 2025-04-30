using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class RFIDLightController : MonoBehaviour
{
    private string apiUrl = "http://nm-rfid-4.new-media-metagame.com:8001";
    private bool commandSent = false;

    void Start()
    {
        StartCoroutine(CheckRFIDInput());
    }

    IEnumerator CheckRFIDInput()
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(apiUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error connecting to RFID API: " + www.error);
            }
            else
            {
                if (!string.IsNullOrEmpty(www.downloadHandler.text) && !commandSent)
                {
                    Debug.Log("RFID Input Detected: " + www.downloadHandler.text);
                    StartCoroutine(SendPulseGreenCommand());
                    commandSent = true;
                }
            }

            yield return new WaitForSeconds(1f); // Check every second
        }
    }

    IEnumerator SendPulseGreenCommand()
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "pulse_green"); // Adjust the field and value according to the API specification

        UnityWebRequest www = UnityWebRequest.Post(apiUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error sending command to RFID API: " + www.error);
        }
        else
        {
            Debug.Log("Pulse green command sent successfully");
        }
    }
}
