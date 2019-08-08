using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using JungleWarServer.NetFrame;
using JungleWarServer.MessageHandle;
using JungleWarServer.Protocol;

namespace JungleWarServer.Handler
{
    public class HandlerCenter
    {
        private MessageHandler message_handler = new MessageHandler();
        public Dictionary<RequestCode, BaseHandler> handler_dict = new Dictionary<RequestCode, BaseHandler>();

        public HandlerCenter()
        {
            InitHandler();
        }

        private void InitHandler()
        {
            handler_dict.Add(RequestCode.USER, new UserHandler(this));
            handler_dict.Add(RequestCode.ROOM, new RoomHandler(this));
            handler_dict.Add(RequestCode.GAME, new GameHandler(this));
            // todo
        }

        /// <summary>
        /// 有客户端的连接到达
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        public void OnClientAccept(int id, UserToken token)
        {
            Console.WriteLine("接收到客户端的连接 id为 " + id);
            message_handler.AddToken(token);
            foreach (BaseHandler handler in handler_dict.Values) handler.OnClientAccept(id, token);
        }

        public void OnClientClose(int id, UserToken token, string message)
        {
            Console.WriteLine("接收到客户端的关闭 id为 " + id);
            Console.WriteLine("提示信息为 " + message);
            message_handler.RemoveToken(token);
            foreach (BaseHandler handler in handler_dict.Values) handler.OnClientClose(id, token, message);
        }

        public void OnClientReceive(int id, UserToken token, byte[] data)
        {
            // Console.WriteLine("接收到客户端的数据 id为 " + id);
            message_handler.AddTokenReceiveData(token, data); // 将接收到的数据添加到缓存中
            HandleTokenData(token);
        }

        private void HandleTokenData(UserToken token)
        {
            byte[] body = null;
            RequestCode request_code = RequestCode.NONE;
            ActionCode action_code = ActionCode.NONE;
            string str = null;
            string result = null;
            BaseHandler handler = null;

            try
            {
                while (true)
                {
                    body = message_handler.GetMessageBody(token);
                    if (body == null) break;
                    request_code = (RequestCode)BitConverter.ToInt32(body, 0);
                    action_code = (ActionCode)BitConverter.ToInt32(body, 4);
                    str = Encoding.UTF8.GetString(body, 8, body.Length - 8);
                    handler_dict.TryGetValue(request_code, out handler);

                    #region test
                    // Console.WriteLine("收到客户端的数据如下");
                    // Console.Write("token_id:" + token.TokenId + " RequestCode:" + Enum.GetName(typeof(RequestCode), request_code) + " ActionCode:" + Enum.GetName(typeof(ActionCode), action_code) + " data:" + str);
                    // Console.WriteLine();
                    #endregion
                    result = handler.Handle(token, action_code, str);
                    // 给客户端发送响应
                    if (result != null)
                        token.Send(MessageHandler.PackData(action_code, result));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
            }
        }
    }
}
