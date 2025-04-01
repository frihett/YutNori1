using System;
using System.Collections.Generic;
using Server.Session;

namespace Server
{
    public class MatchManager
    {
        

        private static MatchManager _match = new MatchManager();
        public static MatchManager Instance { get { return _match; } }

        int _roomId = 0; 


        private List<ClientSession> _waitingQueue = new List<ClientSession>();
        private List<GameRoom> _rooms = new List<GameRoom>();

        public void RequestMatch(ClientSession session)
        {
            lock (_waitingQueue)
            {
                Console.WriteLine($"[MatchManager] Matching Request - SessionID: {session.SessionID}");

                _waitingQueue.Add(session);

                if (_waitingQueue.Count >= 2)
                {
                    GameRoom room = new GameRoom();
                    room.RoomId = _roomId++;
                    _rooms.Add(room);

                    ClientSession sessionA = _waitingQueue[0];
                    ClientSession sessionB = _waitingQueue[1];
                    _waitingQueue.RemoveRange(0, 2);

                    room.Push(() =>
                    {
                        room.Enter(sessionA);
                        room.Enter(sessionB);

                        S_MatchSuccess successA = new S_MatchSuccess();
                        successA.roomId = room.RoomId;
                        successA.playerId = 0;
                        sessionA.Send(successA.Write());

                        S_MatchSuccess successB = new S_MatchSuccess();
                        successB.roomId = room.RoomId;
                        successB.playerId = 1;
                        sessionB.Send(successB.Write());
                    });

                    sessionA.Room = room;
                    sessionB.Room = room;

                    Console.WriteLine($"[MatchManager] New GameRoom created with 2 players: {sessionA.SessionID}, {sessionB.SessionID}");

                    ServerRoomManager.Instance.AddRoom(room);
                }
            }
        }

        public void RemoveRoom(GameRoom room)
        {
            lock (_rooms)
            {
                _rooms.Remove(room);
            }
        }
    }
}
