using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CamApp.Scripts
{
    public class TcpConnector
    {
        private byte[] _buffer { set; get; }
        private Socket _socket { set; get; }
        private IPEndPoint _point;

        public TcpConnector(string ip, int port)
        {
            _point = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _buffer = new byte[4096];
        }

        public void Connect()
        {
            SocketAsyncEventArgs conn = new SocketAsyncEventArgs { RemoteEndPoint = _point };
            conn.Completed += OnConnected;

            _socket.ConnectAsync(conn);
        }

        private void OnConnected(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                Debug.WriteLine("Error on socket " + e.RemoteEndPoint.ToString() + " " + e.SocketError);
            }
            else
            {
                Debug.WriteLine("Connection success");
                StartListen();
            }
        }

        private void StartListen()
        {
            SocketAsyncEventArgs conn = new SocketAsyncEventArgs();
            conn.Completed += OnListen;
            conn.SetBuffer(_buffer, 0, _buffer.Length);

            _socket.ReceiveAsync(conn);
        }

        private void OnListen(object sender, SocketAsyncEventArgs e)
        {

            Debug.WriteLine("RECIEVED FROM SOCKET");
            Debug.WriteLine("--------------------");
            string result = Encoding.UTF8.GetString(_buffer);
            Debug.WriteLine(result);
            Debug.WriteLine("--------------------");
            StartListen();
        }

        public void Send(byte[] buffer)
        {
            SocketAsyncEventArgs conn = new SocketAsyncEventArgs();
            conn.SetBuffer(buffer, 0, buffer.Length);

            _socket.SendAsync(conn);
        }
    }
}
