using JungleWarServer.Protocol;
using JungleWarServer.NetFrame;

namespace JungleWarServer.Handler
{
    public abstract class BaseHandler
    {
        protected RequestCode request_code = RequestCode.NONE;

        /// <summary>
        /// 处理客户端的消息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="action_code"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public abstract string Handle(UserToken token, ActionCode action_code, string str);
        /// <summary>
        /// 有客户端的到达
        /// </summary>
        /// <param name="token_id"></param>
        /// <param name="token"></param>
        public abstract void OnClientAccept(int token_id, UserToken token);
        /// <summary>
        /// 处理客户端的断开
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public abstract void OnClientClose(int id, UserToken token, string message);
    }
}