using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class Program
    {
        static IPHostEntry ipHost = Dns.GetHostEntry("94.233.95.150");
        static IPAddress ipAddress = ipHost.AddressList[0];
        static IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 80);

        static void Main(string[] args)
        {
            getClient(7);
        }

        public static void getClient(int backlog)
        {
            Socket sListener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Byte[] data = new byte[4096];
            String message = null;
            sListener.Bind(ipEndPoint);
            sListener.Listen(backlog);

            while (true)
            {
                Console.WriteLine("Ожидаю подключения на порте {0}", ipEndPoint);

                Socket handler = sListener.Accept();

                Byte[] tData = new byte[1024];

                int byteResponse = handler.Receive(tData);

                string tmessage = Encoding.UTF8.GetString(tData);

                message += tmessage.Substring(0,tmessage.IndexOf("!EOF"))+"\n";
                if (Encoding.UTF8.GetBytes(message).Length>1024)
                {
                    message = message.Substring(message.Length - 512);
                }
                Console.WriteLine(message);
                byte[] response = new byte[1024];

                response = Encoding.UTF8.GetBytes(message);
                handler.Send(response);
                if (tmessage.IndexOf(":sd") > -1)
                {
                    break;
                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
    }
}
