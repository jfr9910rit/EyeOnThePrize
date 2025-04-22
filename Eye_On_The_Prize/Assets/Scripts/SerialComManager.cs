using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialComManager : MonoBehaviour
{
    Thread IOThread = new Thread(DataThread);
    private static SerialPort sp;
    private static string outgoingMsg = "";

    private static void DataThread()
    {
        sp = new SerialPort("COM4", 9600);
        sp.Open();

        while (true)
        {
            if (outgoingMsg != "")
            {
                sp.Write(outgoingMsg);
                outgoingMsg = "";
            }

            Thread.Sleep(200);
        }
    }

    private void OnDestroy()
    {
        sp.Close();
    }

    public void SendSerialMessage(string msg)
    {
        outgoingMsg = msg;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IOThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
