using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System;
using System.Net;
using System.Net.Sockets;

public class Server : MonoBehaviour
{
    public GameObject Cube;
    private static byte[] inBuffer = new byte [512];
    private static IPAddress ip;
    private static Socket server;
    private static IPEndPoint localEP;
    private static IPEndPoint client;
    private static IPHostEntry hostInfo;
    private static EndPoint remoteClient;
    private static int recv = 0;

    //LECTURE 5
    private float[] pos;

    public static void StartServer()
    {
        byte[] buffer = new byte[512];

        hostInfo = Dns.GetHostEntry(Dns.GetHostName());

        //Good practice to iterate through the list to get the desired address
        ip = hostInfo.AddressList[1];

        //Local Host
       // ip = IPAddress.Parse("127.0.0.1");

        Debug.Log($"Server Name: {hostInfo.HostName} | IP: {ip}");

        localEP = new IPEndPoint(ip, 11111);

        server = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

        //Create an End point to capture the info from the sending client
        client = new IPEndPoint(IPAddress.Any, 0); //0 (any available port)
        remoteClient = (EndPoint)client;

        //Bind and then recieve data

        try
        {
            //Bind local end point
            server.Bind(localEP);

            Console.WriteLine("Waiting for data...");

            ////infinite loop until server is closed
            //while (true)
            //{
            //    int recv = server.ReceiveFrom(buffer, ref remoteClient);

            //    //Prints the endpoint as a string
            //   // Console.WriteLine("Recieved from: {0}", remoteClient.ToString());

            //   // Console.WriteLine("Data: {0}", Encoding.ASCII.GetString(buffer, 0, recv));



            //    //If client sends floats
            //    // Console.WriteLine("Data {0}", BitConverter.ToSingle(buffer, 0));



            //}

            //  server.Shutdown(SocketShutdown.Both);
            //  server.Close();
        }

        catch (Exception e)
        {
            Debug.Log("Exception" + e.ToString());
        }

    }


     // Start is called before the first frame update
     void Start()
    {
        Cube = GameObject.Find("Cube");
        StartServer();

        //Lecture5
        //Non-blocking mode
        server.Blocking = false;

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
           //Recieve Data from Client for Poxition.x
           recv = server.ReceiveFrom(inBuffer, ref remoteClient);
        }
        catch (SocketException e)
        {
            Debug.Log("Exception" + e.ToString());
        }
      
        //  Debug.Log("Recvied x position" + Encoding.ASCII.GetString(inBuffer, 0, recv) + "Sent from the client" + remoteClient.ToString());

        //Parse the bytes containing cubes position to a float
        //
        //   float x = float.Parse(Encoding.ASCII.GetString(inBuffer, 0, recv));

        //Recieves Z position
        //   recv = server.ReceiveFrom(inBuffer, ref remoteClient);

        //     Debug.Log("Recvied z position" + Encoding.ASCII.GetString(inBuffer, 0, recv) + "Sent from the client" + remoteClient.ToString());

        //Parse the bytes containing cubes position to a float
        //   float z = float.Parse(Encoding.ASCII.GetString(inBuffer, 0, recv));

        //Move the position of the cube based off the original position of the cube in the client
        // Cube.transform.position = new Vector3(x, Cube.transform.position.y, z);


        //LECTURE 5
        //4 bytes will give you the number of elements in the float array so we divide the recieved data by 4
        pos = new float[recv / 4];
        Buffer.BlockCopy(inBuffer, 0, pos, 0, recv);

        Cube.transform.position = new Vector3(pos[0], pos[1], pos[2]);




    }
}
