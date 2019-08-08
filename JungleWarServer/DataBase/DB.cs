using System;
using System.Collections.Generic;
using JungleWarServer.NetFrame;

namespace JungleWarServer.DataBase
{
    public class DB
    {
        // 存储已登录的客户端对应的登录用户名
        // 在登录的同时添加 下线的同时删除
        private static Dictionary<UserToken, string> token_2_username = new Dictionary<UserToken, string>();
        // 存储所有用户信息
        private static Dictionary<string, UserInfo> username_2_info = new Dictionary<string, UserInfo>();

        static DB()
        {
            UserInfo info = new UserInfo("123", "123");
            username_2_info.Add("123", info);
            UserInfo info2 = new UserInfo("111", "111");
            username_2_info.Add("111", info2);
            UserInfo info3 = new UserInfo("222", "222");
            username_2_info.Add("222", info3);
        }

        public static bool AddUser(string username, string password)
        {
            bool is_contain = username_2_info.ContainsKey(username);
            if (is_contain)
                return false;
            username_2_info.Add(username, new UserInfo(username, password));
            return true;
        }

        public static bool UserLogin(UserToken token, string username)
        {
            bool is_contain = token_2_username.ContainsKey(token);
            if(is_contain) return false;
            token_2_username.Add(token, username);
            return true;
        }

        public static UserInfo GetUserInfo(string name)
        {
            UserInfo info = null;
            username_2_info.TryGetValue(name, out info);
            return info;
        }
        public static UserInfo GetUserInfo(UserToken token)
        {
            string name = null;
            token_2_username.TryGetValue(token, out name);
            UserInfo info = null;
            username_2_info.TryGetValue(name, out info);
            return info;
        }

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool VerifyUser(string username, string password)
        {
            UserInfo info = null;
            username_2_info.TryGetValue(username, out info);
            if (info == null) return false;
            else if (info.password == password) return true;
            else return false;
        }
    }
}