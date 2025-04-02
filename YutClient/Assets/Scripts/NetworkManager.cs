using System.Net;
using UnityEngine;
using ServerCore;
using DummyClient;
using System.Net.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    ServerSession _session = new ServerSession();

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }

    void Start()
    {
        UnityEngine.Debug.Log($" Unity 메인 스레드 ID: {Thread.CurrentThread.ManagedThreadId}");

        // DNS
        string host = Dns.GetHostName();

        IPHostEntry ipHost = Dns.GetHostEntry(host);
        // IPAddress iPAdress = ipHost.AddressList[0];

        IPAddress iPAdress = IPAddress.Parse("192.168.244.241");

        IPEndPoint endPoint = new IPEndPoint(iPAdress, 7777);

        Connector connector = new Connector();
        connector.Connect(endPoint, () => { return _session; }, 1);



    }

    void Update()
    {
        List<IPacket> list = PacketQueue.instance.PopAll();
        foreach (IPacket packet in list)
        {
            PacketManager.Instance.HandlePacket(_session, packet);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


}
