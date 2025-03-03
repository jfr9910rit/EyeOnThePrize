using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class Controller : MonoBehaviour
{
    Thread IOThread = new Thread(DataThread);
    private static SerialPort sp;
    private static string incomingMsg = "";
    //private static string outgoingMsg = "";

    private static void DataThread()
    {
        sp = new SerialPort("COM3", 9600);
        sp.Open();

        while(true)
        {
            incomingMsg = sp.ReadExisting();
            Thread.Sleep(50);
        }
    }

    private void OnDestroy()
    {
        IOThread.Abort();
        sp.Close();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IOThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (incomingMsg != "")
        {
            switch (incomingMsg)
            {
                case "RED":
                    break;
                case "YEL":
                    break;
                case "GRE":
                    break;
                case "BLU":
                    break;
            }
        }
    }
}
