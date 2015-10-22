using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CamApp.Scripts
{
    public class SocketController
    {
        
        private static TcpConnector connector;

        public static void OpenSocket()
        {
            connector = new TcpConnector("192.168.42.1", 7878);
            connector.Connect();
        }
        
        public static void GetConfigs()
        {
            string str = "{\"msg_id\":257,\"token\":0}";
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            connector.Send(bytes);
        }
        
        public static void GetSettings()
        {
            string str = "{\"msg_id\":3,\"token\":1}";
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            connector.Send(bytes);
        }
    }
}
