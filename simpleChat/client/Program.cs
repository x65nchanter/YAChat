using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HTML_cycles
{
    class Program
    {
        static IPHostEntry ipHost = Dns.GetHostEntry("94.233.95.150");
        static IPAddress ipAddress = ipHost.AddressList[0];
        static IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 80);

        static void Main(string[] args)
        {
            Console.WriteLine("Введите логин");
            string login = Console.ReadLine();
            while (true)
            {
                Console.WriteLine("Введите сообщение: ");
                string message = " "+login+ ": " + Console.ReadLine()+"!EOF";

                byte[] data = Encoding.UTF8.GetBytes(message);
                int i;
                int x;
                byte[] response = requestHTTP(data, out x, out i);
                Console.Clear();
                Console.WriteLine("Сообщение от сервера: ");
                Console.WriteLine(Encoding.UTF8.GetString(response));
                if (message.IndexOf(":q") > -1)
                {
                    break;
                }
            }

        }

        public static byte[] requestHTTP(byte[] message, out int byteSend, out int byteResponse)
        {
            byte[] response = new byte[1024];
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(ipEndPoint);
            byteSend = sender.Send(message);
            byteResponse = sender.Receive(response);
            return response;
        }
    }
    enum StandarstsPorts
    {
        HTTP = 80,
        HTTPS = 8080,
    }

}
