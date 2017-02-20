using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FlatBuffers;
using sample;
using System.Net;

public class netchan 
{
    public Int32 protocol;
    public Int32 sequence;
    public Int32 ack;
    public Vector3 pos;

    public netchan(Int32 p,Int32 s,Int32 a, float x, float y, float z)
    {
        protocol = p;
        sequence = s;
        ack = a;
        pos.x = x;
        pos.y = y;
        pos.z = z;
    }
}

public class Network : MonoBehaviour {


    
    public byte[] bufin;
    public byte[] bufout;
    public Transform player;
    public Transform[] networkedPlayers;
    private IPEndPoint ep;
    private System.Net.Sockets.UdpClient client;

    public byte[] SerializeNetchan(int seq, int ack)
    {

        var builder = new FlatBufferBuilder(1);

        var plays = new Offset<Player>[1];
        Player.StartPlayer(builder);
        Player.AddId(builder, 1);
        var pos = Vec3.CreateVec3(builder, player.position.x, player.position.y, player.position.z);
        Player.AddPos(builder, pos);
        var rot = Vec3.CreateVec3(builder, player.localEulerAngles.x, player.localEulerAngles.y, player.localEulerAngles.z);
        Player.AddRot(builder, rot);
        var p1 = Player.EndPlayer(builder);
        plays[0] = p1;

        // serialize FlatBuffer data
        var players = Netchan.CreatePlayersVector(builder, plays);

        Netchan.StartNetchan(builder);
        Netchan.AddProtocol(builder, 1234);
        Netchan.AddSequence(builder, seq);
        Netchan.AddAck(builder, ack);
        Netchan.AddPlayers(builder, players);
        var net = Netchan.EndNetchan(builder);
        builder.Finish(net.Value);
        var buf = builder.SizedByteArray();
        
        return buf;
    }

    public Netchan deserializeNetchan(byte[] buf)
    {
        var bb = new ByteBuffer(buf);

        var netchan = Netchan.GetRootAsNetchan(bb);

        //print(netchan.PlayersLength);

        /*var protocol = netchan.Protocol;
        var sequence = netchan.Sequence;
        var ack = netchan.Ack;
        var player = (Player) netchan.Players;
        var vec3 = (Vec3)player.Pos;
        var x = vec3.X;
        var y = vec3.Y;
        var z = vec3.Z;
        var n = new netchan(protocol, sequence, ack, x, y, z);
        */
        return netchan;
    }

	// Use this for initialization
	void Start () {
        client = new System.Net.Sockets.UdpClient();
        ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        client.Connect(ep);
        InvokeRepeating("sendPos", 0.0f, 0.0167f);
    }
	
    void sendPos()
    {
        bufin = SerializeNetchan(1, 2);
        Netchan net = deserializeNetchan(bufin);
        print("Sending...\nProtocol: " + net.Protocol +
           "\nSequence: " + net.Sequence + "\nAck: " + net.Ack);

        client.Send(bufin, bufin.Length);

        bufout = client.Receive(ref ep);
        net = deserializeNetchan(bufout);
        Player p = net.Players(0).Value;
         print("Receiving...\nProtocol: " + net.Protocol +
             "\nSequence: " + net.Sequence + "\nAck: " + net.Ack +
             "\n x: " + p.Rot.Value.X +
             "\n y: " + p.Rot.Value.Y +
             "\n z: " + p.Rot.Value.Z);
         print(net.ByteBuffer.Data);
         
        var i = 0;
        foreach (Transform t in networkedPlayers)
        {
            i++;
            t.position = new Vector3(p.Pos.Value.X, p.Pos.Value.Y, (float)p.Pos.Value.Z + 2*i);
            t.eulerAngles = new Vector3(p.Rot.Value.X, p.Rot.Value.Y, p.Rot.Value.Z);
            //t.rotation = new Quaternion(p.Rot.Value.X, p.Rot.Value.Y, p.Rot.Value.Z, 0);
        }
        
    }

	// Update is called once per frame
	void Update () {
        
    }
    
}
