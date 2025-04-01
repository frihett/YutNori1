using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Session;
using ServerCore;

namespace Server
{
    public class GameRoom : IJobQueue
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        public int RoomId { get; set; }
        public int CurrentTurnPlayerId = 0;



        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
                foreach(ClientSession s in _sessions)
               {
                   s.Send(_pendingList);
               }
            // Console.WriteLine($"Flushed {_pendingList.Count} items");
            _pendingList.Clear();
        }
        public void Broadcast(ArraySegment<byte> segment)
        {
         
            _pendingList.Add(segment);
             
        }
        public void Enter(ClientSession session)
        {
            // 플레이어를 추가
                _sessions.Add(session);
                session.Room = this;
            // 신입생한테 모든 플레이어 목록 전송
            S_PlayerList players = new S_PlayerList();
            foreach(ClientSession s in _sessions)
            {
                players.players.Add(new S_PlayerList.Player()
                {
                    isSelf =( s ==  session),
                    playerId = s.SessionID,
                   

                });
            }
            session.Send(players.Write());

            // 신입생 입장을 모두에게 알린다

            S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
            enter.playerId = session.SessionID;
            
            Broadcast(enter.Write());

        }
        public void Leave(ClientSession session)
        {   
            // 플레이어 제거 
            _sessions.Remove(session);

            // 모두에게 알린다
            S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
            leave.playerId = session.SessionID;
            Broadcast(leave.Write());

        }

        public void ProcessThrowYut(ClientSession session, C_ThrowYut pkt)
        {
            Random rand = new Random();
            int result = rand.Next(1, 6); // 1~5

            S_YutResult resultPkt = new S_YutResult();
            resultPkt.playerId = pkt.playerId;
            resultPkt.value = result;

            Broadcast(resultPkt.Write());
        }
        public void ChangeTurn(int prevPlayerId)
        {
            int nextTurn = (prevPlayerId == 0) ? 1 : 0;

            CurrentTurnPlayerId = nextTurn;

            S_TurnChange pkt = new S_TurnChange();
            pkt.playerId = nextTurn;

            Broadcast(pkt.Write());
        }
        public void ProcessMovePiece(C_MovePiece pkt)
        {

            // 이동 결과를 모두에게 전송
            S_MovePiece movePkt = new S_MovePiece();
            movePkt.playerId = pkt.playerId;
            movePkt.pieceIndex = pkt.pieceIndex;
            movePkt.startIndex = pkt.startIndex;
            movePkt.distance = pkt.distance;

            Broadcast(movePkt.Write());
        }

        public void BroadcastGameWin(int winnerId)
        {
            S_GameWin pkt = new S_GameWin();
            pkt.winnerId = winnerId;

            Broadcast(pkt.Write());
        }


    }
}
