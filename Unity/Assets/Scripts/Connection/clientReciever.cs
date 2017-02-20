using UnityEngine;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


// This class is just for receiving messages from the server
public class clientReciever : MonoBehaviour {

    public Transform[] remChars = new Transform[3];

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        byte[] recv = Receive();
        Console.Write("message recieved");
        if (recv != null)
        {
            Debug.Log("Receive : " + Encoding.Default.GetString(recv));
            //name.text = "Receive : " + Encoding.Default.GetString(recv);
            recv = null;
        }
    }

    public void OnApplicationQuit()
    {
        Disconnect();

        Destroy(this.gameObject);
    }

    //--------------------------------------------------------------------------------------------------------------------------------
    // Network
    //--------------------------------------------------------------------------------------------------------------------------------
    public string ip = "127.0.0.1";
    public int port = 25565;

    private Socket m_socket = null;
    private Thread m_thread = null;
    private Queue<byte[]> m_receives = new Queue<byte[]>();

    public bool Connect()
    {
        // setup socket
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // connect socket
        try
        {
            IPAddress ipAddr = System.Net.IPAddress.Parse(ip);
            IPEndPoint ipEndPoint = new System.Net.IPEndPoint(ipAddr, port);
            m_socket.Connect(ipEndPoint);
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket connect error! : " + socketException.ToString());
            return false;
        }

        // start recv thread        
        if (m_thread == null)
        {
            m_thread = new Thread(() =>
            {
                while (m_socket != null)
                {
                    // recv add queue
                    byte[] buffer = new byte[512];
                    m_socket.Receive(buffer);
                    lock (m_receives)
                    {
                        m_receives.Enqueue(buffer);
                    }
                }
                lock (m_thread)
                {
                    m_thread = null;
                }
            }
            );
        }
        m_thread.Start();

        return true;
    }

    public void Disconnect()
    {
        // close socket
        if (m_socket != null)
        {
            m_socket.Close();
            m_socket = null;
        }

        // waiting thread
        if (m_thread != null)
        {
            m_thread.Join();
            m_thread = null;
        }
    }

    public void Send(byte[] buffer)
    {
        if (m_socket == null || buffer.Length <= 0)
        {
            return;
        }

        m_socket.Send(buffer, buffer.Length, 0);
    }

    public void Send(string str)
    {
        byte[] buffer = Encoding.Default.GetBytes(str);

        if (m_socket == null || buffer.Length <= 0)
        {
            return;
        }

        m_socket.Send(buffer, buffer.Length, 0);
    }

    public byte[] Receive()
    {
        lock (m_receives)
        {
            if (m_receives.Count > 0)
            {
                return m_receives.Dequeue();
            }
            return null;
        }
    }
}
