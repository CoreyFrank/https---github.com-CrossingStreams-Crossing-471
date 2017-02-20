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

public class clientConnection: MonoBehaviour
{
    public Transform[] remChars = new Transform[3];
    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
    

    public void Start()
    {
        
    }
 
    public void Awake()
    {
        Application.runInBackground = true;
        Connect();
        //DontDestroyOnLoad(this.gameObject);
		Send(GetBytes("Hello World"));
    }

    public void Update()
    {
        byte[] pos;
        //if (Input.GetKey("space")) { 
            // setup byte array to send to server
            float[] posArray = new float[3];
            posArray[0] = remChars[1].position.x;
            posArray[1] = remChars[1].position.y;
            posArray[2] = remChars[1].position.z;
            //Debug.Log(pos.ToString());
            pos = new byte[posArray.Length * sizeof(float)];
            Buffer.BlockCopy(posArray, 0, pos, 0, pos.Length);
            Send(pos);
       // }

        string msg;
        byte[] recv = Receive();
        if (recv != null)
        {
            
            msg = GetString(recv).Trim();
            Debug.Log(msg);

            if (msg.IndexOf("Hello World") == 0)
            {
                Debug.Log("Server Connection Successful: " + msg);
            }
            else if (msg.IndexOf("pos") == 0)
            {
                Debug.Log("Sending pos " + msg);
            }
            else
            {

                float[] posv= GetFloatArray(recv);
                Vector3 p = new Vector3(posv[0], posv[1], posv[2]);
                remChars[0].position = p;
                Debug.Log(posv[0]);
                Debug.Log(posv[1]);
                Debug.Log(posv[2]);
                Debug.Log("Something Went wrong " + msg);
                Debug.Log(msg.Length);
                Debug.Log("Hello World".Length);

            }
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

    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    static float[] GetFloatArray(byte[] bytes)
    {
        float[] floats = new float[bytes.Length / sizeof(float)];
        System.Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);
        return floats;
    }

    public void Send(string str)
    {
        byte[] buffer = GetBytes(str);
 
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
