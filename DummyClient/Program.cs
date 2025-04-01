using ServerCore;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
   
    class Program
    {
        static void Main(string[] args)
        {
            // DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            // IPAddress iPAdress = ipHost.AddressList[0];
            IPAddress iPAdress = IPAddress.Parse("125.128.222.150");
            IPEndPoint endPoint = new IPEndPoint(iPAdress, 7777);


            Connector connector = new Connector();
            connector.Connect(endPoint, () => { return SessionManager.instance.Generate(); },10);

            while (true)
            {

                try
                {
                    SessionManager.instance.SendForEach();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Thread.Sleep(250);
            }



        }
    }
}