using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using Server.Session;

namespace Server
{
   
    class Program
    {
        static Listener _listener = new Listener();
        public static GameRoom Room = new GameRoom();

        static void FlushRoom()
        {
            ServerRoomManager.Instance.FlushAllRooms();
            JobTimer.instance.Push(FlushRoom, 250);

        }

        static void Main(string[] args)
        {

            // DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            // IPAddress iPAdress = ipHost.AddressList[0];

            IPAddress iPAdress = IPAddress.Any;
            // IPAddress iPAdress = IPAddress.Parse("119.192.110.86");

            IPEndPoint endPoint = new IPEndPoint(iPAdress, 7777);



            _listener.init(endPoint, () => { return SessionManager.instance.Generate(); });
            Console.WriteLine("Listening ...");

            // FlushRoom();
            JobTimer.instance.Push(FlushRoom);
            while (true)
            {
                JobTimer.instance.Flush();
            }



        }
    }
}