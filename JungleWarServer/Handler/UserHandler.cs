using System;
using JungleWarServer.NetFrame;
using JungleWarServer.Protocol;
using JungleWarServer.DataBase;

namespace JungleWarServer.Handler
{
    public class UserHandler : BaseHandler
    {
        private static int register_count = 0;
        private HandlerCenter center = null;
        public UserHandler(HandlerCenter center)
        {
            request_code = RequestCode.USER;
            this.center = center;
        }

        public override string Handle(UserToken token, ActionCode action_code, string str)
        {
            string result = null;
            switch (action_code)
            {
                case ActionCode.LOGIN:
                    result = Login(token, str);
                    break;
                case ActionCode.REGISTER:
                    result = Register(str);
                    break;
                default:
                    break;
            }

            return result;
        }

        public override void OnClientAccept(int token_id, UserToken token)
        {
        }

        public override void OnClientClose(int id, UserToken token, string message)
        {
            // todo
        }

        private string Login(UserToken token, string str)
        {
            Console.WriteLine("进入到登录方法 " + str);
            string[] strs = str.Split(',');
            bool is_get = DB.VerifyUser(strs[0], strs[1]); // 是否有该用户
            if (is_get == true)
            {
                DB.UserLogin(token, strs[0]);
                UserInfo info = DB.GetUserInfo(strs[0]);
                // 返回 Success,用户名,总场数,胜利场数
                return "Success" + "," + strs[0] + "," + info.totalcount + "," + info.wincount;
            }
            else
                return "Fail";
        }

        private string Register(string str)
        {
            Console.WriteLine("进入到注册方法 " + str);
            string[] strs = str.Split(',');
            bool is_success = DB.AddUser(strs[0], strs[1]);
            if (is_success) return "Success";
            else return "Fail";
        }
    }
}