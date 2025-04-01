using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance{ get{ return _instance;   } }
    #endregion

    PacketManager()
    {
        Register();
    }

    Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
    Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    public void Register()
    {
      _makeFunc.Add((ushort)PacketID.S_BroadcastEnterGame, MakePacket<S_BroadcastEnterGame>);
        _handler.Add((ushort)PacketID.S_BroadcastEnterGame, PacketHandler.S_BroadcastEnterGameHandler);
      _makeFunc.Add((ushort)PacketID.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
        _handler.Add((ushort)PacketID.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);
      _makeFunc.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
        _handler.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);
      _makeFunc.Add((ushort)PacketID.S_MatchSuccess, MakePacket<S_MatchSuccess>);
        _handler.Add((ushort)PacketID.S_MatchSuccess, PacketHandler.S_MatchSuccessHandler);
      _makeFunc.Add((ushort)PacketID.S_YutResult, MakePacket<S_YutResult>);
        _handler.Add((ushort)PacketID.S_YutResult, PacketHandler.S_YutResultHandler);
      _makeFunc.Add((ushort)PacketID.S_TurnChange, MakePacket<S_TurnChange>);
        _handler.Add((ushort)PacketID.S_TurnChange, PacketHandler.S_TurnChangeHandler);
      _makeFunc.Add((ushort)PacketID.S_MovePiece, MakePacket<S_MovePiece>);
        _handler.Add((ushort)PacketID.S_MovePiece, PacketHandler.S_MovePieceHandler);
      _makeFunc.Add((ushort)PacketID.S_GameWin, MakePacket<S_GameWin>);
        _handler.Add((ushort)PacketID.S_GameWin, PacketHandler.S_GameWinHandler);

     }
    public void OnRecevPacket(PacketSession session , ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
    {
        ushort count = 0;
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        
        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if (_makeFunc.TryGetValue(id, out func))
        {
            IPacket packet = func.Invoke(session, buffer);
            if (onRecvCallback != null)
                onRecvCallback.Invoke(session, packet);
            else
                HandlePacket(session, packet);
        }


            
    }

    T MakePacket<T>(PacketSession session,ArraySegment<byte> buffer) where T : IPacket,new()
    {
        T pkt = new T();
        pkt.Read(buffer);
        return pkt;
    }

    public void HandlePacket(PacketSession session, IPacket packet)
    {
        Action<PacketSession, IPacket> action = null;
        if (_handler.TryGetValue(packet.Protocol, out action))
            action.Invoke(session, packet);
    }
}