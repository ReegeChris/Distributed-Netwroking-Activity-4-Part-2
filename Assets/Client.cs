using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    public GameObject myCube;
    private static byte[] outBuffer = new byte[512];
    private static IPEndPoint remoteEP;
    private static Socket clientSoc;

    public static void StartClient()
    {
        //Recieve buffer
        byte[] buffer = new byte[512];

        try
        {

            IPAddress ip = IPAddress.Parse("192.168.86.52");
            remoteEP = new IPEndPoint(ip, 11111);

            clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            
        
        } catch (Exception e)
          {
            Debug.Log("Exception: " + e.ToString());
          }


    }

    // Start is called before the first frame update
    void Start()
    {
        myCube = GameObject.Find("Cube");
        StartClient();


    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Sending X: " + myCube.transform.position.x);
        outBuffer = Encoding.ASCII.GetBytes(myCube.transform.position.ToString());

        clientSoc.SendTo(outBuffer, remoteEP);

    }
}
