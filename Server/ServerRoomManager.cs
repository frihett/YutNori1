using System;
using System.Collections.Generic;

namespace Server
{
    public class ServerRoomManager
    {
        

        private static ServerRoomManager _manager = new ServerRoomManager();
        public static ServerRoomManager Instance { get { return _manager; } }

        private List<GameRoom> _rooms = new List<GameRoom>();
        private object _lock = new object();

        public void AddRoom(GameRoom room)
        {
            lock (_lock)
            {
                _rooms.Add(room);
                Console.WriteLine("[ServerRoomManager] GameRoom 등록 완료");
            }
        }

        public void RemoveRoom(GameRoom room)
        {
            lock (_lock)
            {
                _rooms.Remove(room);
                Console.WriteLine("[ServerRoomManager] GameRoom 제거 완료");
            }
        }

        public void BroadcastToAll(ArraySegment<byte> data)
        {
            lock (_lock)
            {
                foreach (var room in _rooms)
                {
                    room.Push(() => room.Broadcast(data));
                }
            }
        }

        public void FlushAllRooms()
        {
            lock (_lock)
            {
                foreach (var room in _rooms)
                {
                    room.Push(() => room.Flush());
                }
            }
        }
    }
}
