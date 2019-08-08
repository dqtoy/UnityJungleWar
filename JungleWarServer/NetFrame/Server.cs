using System;
using System.Net;
using System.Net.Sockets;

namespace JungleWarServer.NetFrame
{
    public class Server
    {
        private Socket socket = null;

        private event Action<int, UserToken> client_accept;
        private event Action<int, UserToken, byte[]> client_receive;
        private event Action<int, UserToken, string> client_close;

        /// <summary>
        /// 创建服务器类
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        public Server(string ip, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(0);
        }

        #region 注册事件
        public void RegisterAccpetEvent(Action<int, UserToken> accept_event)
        {
            client_accept += accept_event;
        }
        public void RegisterReceiveEvent(Action<int, UserToken, byte[]> receive_event)
        {
            client_receive += receive_event;
        }
        public void RegisterCloseEvent(Action<int, UserToken, string> close_event)
        {
            client_close += close_event;
        }
        #endregion

        /// <summary>
        /// 服务器开始运作
        /// </summary>
        public void Start()
        {
            if (socket == null)
            {
                Console.WriteLine("未初始化服务器套接字");
                return;
            }
            BeginAccept();
        }

        // 开始异步接收客户端的连接请求
        private void BeginAccept()
        {
            socket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket client_socket = socket.EndAccept(ar);
            UserToken token = new UserToken(client_socket);
            token.RegisterAccpetEvent(client_accept);
            token.RegisterReceiveEvent(client_receive);
            token.RegisterCloseEvent(client_close);
            token.Start();

            BeginAccept();
        }
    }
}