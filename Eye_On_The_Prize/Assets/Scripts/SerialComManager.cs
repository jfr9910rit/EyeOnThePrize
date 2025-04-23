using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialComManager : MonoBehaviour
{
    [SerializeField] private string COM_A;
    [SerializeField] private string COM_B;
    [SerializeField] private string COM_C;

    Thread IOThread = new Thread(DataThread);
    private static SerialPort sp1;
    private static SerialPort sp2;
    private static SerialPort sp3;
    private static string targetCom = "";
    private static string outgoingMsg = "";

    private static void DataThread()
    {
        sp1 = new SerialPort("COM4", 9600);
        sp1.Open();
        sp2 = new SerialPort("COM9", 9600);
        sp2.Open();
        //sp3 = new SerialPort("COM_", 9600);
        //sp3.Open();

        while (true)
        {
            // if msg exists
            if (outgoingMsg != "" && targetCom != "")
            {
                // send msg to correct port
                switch (targetCom)
                {
                    case "COM4":
                        sp1.Write(outgoingMsg);
                        break;
                    case "COM9":
                        sp2.Write(outgoingMsg);
                        break;
                }

                // clear msg fields
                outgoingMsg = "";
                targetCom = "";
            }

            Thread.Sleep(200);
        }
    }

    private void OnDestroy()
    {
        sp1.Close();
        sp2.Close();
        sp3.Close();
    }

    /*
     * apparently the unity editor wont let functions attached to buttons have more than one parameter so this is how we're doing it.
     * formatted like this: "COM[port#]_[msg]"
     */
    public void SendSerialMessage(string port_msg) 
    {
        string com = port_msg.Substring(0, 4);
        string msg = port_msg.Substring(4);
        targetCom = com;
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
