using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace DummyClient
{
    class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            //PacketManager.Instance.OnRecevPacket(this, buffer);
            PacketManager.Instance.OnRecevPacket(this, buffer, (s, p) => PacketQueue.instance.Push(p));

        }

        public override void OnSend(int numOfByte)
        {
            // Console.WriteLine($"Transferred bytes: {numOfByte}");

        }
    }
}
