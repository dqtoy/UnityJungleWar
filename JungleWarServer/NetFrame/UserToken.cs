using System;
using System.Net.Sockets;

namespace JungleWarServer.NetFrame
{
    public class UserToken
    {
        private Socket socket = null; // 客户端套接字
        private static int buffer_size = 2048; // 缓冲区大小
        private byte[] buffer = new byte[buffer_size]; // 数据缓冲区

        private static int id = -1;
        private int token_id;
        public int TokenId { get { return token_id; } }
        private event Action<int, UserToken> client_accept;
        private event Action<int, UserToken, byte[]> client_receive;
        private event Action<int, UserToken, string> client_close;

        public UserToken(Socket socket)
        {
            this.socket = socket;
            token_id = ++id;
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
        /// 开始接受该客户端的数据
        /// </summary>
        public void Start()
        {
            client_accept(token_id, this);
            BeginReceiveAsync();
        }

        private void BeginReceiveAsync()
        {
            socket.BeginReceive(buffer, 0, buffer_size, SocketFlags.None, ReceiveCallBack, null);
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;

                int count = socket.EndReceive(ar);

                if (count == 0)
                {
                    Close();
                    client_close(token_id, this, "接收到数据长度为0的数据"); // 通知上层有客户端断开
                }

                byte[] data_array = new byte[count];
                Buffer.BlockCopy(buffer, 0, data_array, 0, count);
                client_receive(token_id, this, data_array); // 通知上层有数据到达

                BeginReceiveAsync();
            }
            catch (Exception e)
            {
                Close();
                client_close(token_id, this, e.ToString()); // 通知上层有客户端断开
            }
        }

        /// <summary>
        /// 向该客户端发送数据
        /// </summary>
        /// <param name="data">要发送的字节数组</param>
        public void Send(byte[] data)
        {
            if (socket != null && socket.Connected == true)
                socket.Send(data);
        }

        // 断开此客户端
        private void Close()
        {
            if (socket != null && socket.Connected == true) socket.Close();
        }
    }
}