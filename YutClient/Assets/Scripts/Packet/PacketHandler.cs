using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


    class PacketHandler
    {
     
        public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
        {
        S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
            ServerSession serverSession = session as ServerSession;
       
        }
        public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
        {
            S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
            ServerSession serverSession = session as ServerSession;

        }
         public static void S_PlayerListHandler(PacketSession session, IPacket packet)
        {
            S_PlayerList chatPacket = packet as S_PlayerList;
            ServerSession serverSession = session as ServerSession;

        }
       
    
    
      public static void S_MatchSuccessHandler(PacketSession session, IPacket packet)
    {
        S_MatchSuccess match = packet as S_MatchSuccess;

        UnityEngine.Debug.Log(" ddddd...");
        UnityEngine.Debug.Log($" 현재 스레드 ID: {Thread.CurrentThread.ManagedThreadId}");

        GameManager.Instance.PlayerId = match.playerId;
       
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public static void S_YutResultHandler(PacketSession session, IPacket packet)
    {
        UnityEngine.Debug.Log($" YutResult got");

        S_YutResult pkt = packet as S_YutResult;
        ServerSession serverSession = session as ServerSession;

        
        if (pkt.playerId != GameManager.Instance.PlayerId)
            return;

     

        // 이동 표시
        GameManager.Instance.moveUI.ShowMoveOptions(pkt.value);
    }

    public static void S_TurnChangeHandler(PacketSession session, IPacket packet)
    {
        S_TurnChange pkt = packet as S_TurnChange;
        GameManager.Instance.SetTurn(pkt.playerId);
    }

    public static void S_MovePieceHandler(PacketSession session, IPacket packet)
    {
        S_MovePiece pkt = packet as S_MovePiece;
        int dist;

        Piece piece = GameManager.Instance.GetPiece(pkt.playerId, pkt.pieceIndex);
        if (piece != null)
        {

            piece.currentIndex = pkt.startIndex;
            dist = pkt.distance;

            if (piece.currentIndex == -1)
                piece.StartMoveFromStart(dist);
            else
                piece.EnqueueMove(dist);
        }
    }

    public static void S_GameWinHandler(PacketSession session, IPacket packet)
    {
        S_GameWin pkt = packet as S_GameWin;

        GameManager.Instance.ShowWinUI(pkt.winnerId);
    }

}
