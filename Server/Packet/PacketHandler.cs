using Server;
using Server.Session;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PacketHandler
{
    public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        
        if(clientSession.Room == null)
        {
            return;
        }
        GameRoom room = clientSession.Room;
        room.Push(() => room.Leave(clientSession));
    }
   
   
    
     public static void C_MatchRequestHandler(PacketSession session, IPacket packet)
    {
        //ClientSession clientSession = session as ClientSession;
        //if (clientSession.Room == null)
        //    return;

        //GameRoom room = clientSession.Room;
        //room.Push(() => room.Jump(clientSession));
        ClientSession clientSession = session as ClientSession;
        MatchManager.Instance.RequestMatch(clientSession);
    }

    public static void C_ThrowYutHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_ThrowYut pkt = packet as C_ThrowYut;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;

        room.Push(() => room.ProcessThrowYut(clientSession, pkt));
    }

    public static void C_TurnChangeHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_TurnChange pkt = packet as C_TurnChange;

        GameRoom room = clientSession.Room;
        if (room == null) return;

        room.Push(() => room.ChangeTurn(pkt.playerId));
    }

    public static void C_MovePieceHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_MovePiece pkt = packet as C_MovePiece;

        if (clientSession.Room == null)
            return;

        GameRoom room = clientSession.Room;

        room.Push(() => room.ProcessMovePiece(pkt));
    }

    public static void C_GameWinHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;
        C_GameWin pkt = packet as C_GameWin;

        GameRoom room = clientSession.Room;
        if (room == null)
            return;

        room.Push(() =>
        {
            //room.BroadcastGameWin(pkt.playerId);
            room.Push(() => room.BroadcastGameWin(pkt.playerId));
        });
    }

}

