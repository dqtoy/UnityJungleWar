using System.Threading;
using System.Text;
using System.Collections.Generic;
using JungleWarServer.NetFrame;
using JungleWarServer.DataBase;
using JungleWarServer.Protocol;
using JungleWarServer.MessageHandle;

namespace JungleWarServer.Handler
{
    public class Room
    {
        public RoomState state = RoomState.WAIT; // 房间状态
        public UserToken host { get; set; } // 房主
        public int host_id // 房主ID
        {
            get
            {
                if (host != null) return host.TokenId;
                else return -1;
            }
        }
        public List<UserToken> players_list = new List<UserToken>();

        public int host_hp = 100;
        public int normal_hp = 100;

        public Room(){ }

        /// <summary>
        /// 返回房主信息
        /// 格式为 username,totalcount,wincount
        /// </summary>
        /// <returns></returns>
        public string GetHostInfo()
        {
            UserInfo host_info = DB.GetUserInfo(host);
            string result = host_id + "," + host_info.username + "," + host_info.totalcount + "," + host_info.wincount;
            return result;
        }

        /// <summary>
        /// 返回房间内所有玩家的信息
        /// </summary>
        /// <returns>id,username,totalcount,wincount|id,username,totalcount,wincount</returns>
        public string GetAllPlayerInfo()
        {
            StringBuilder result = new StringBuilder();
            foreach (UserToken player in players_list)
            {
                UserInfo info = DB.GetUserInfo(player);
                result.Append(player.TokenId + "," + info.username + "," + info.totalcount + "," + info.wincount + "|");
            }
            result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        // 向房间内所有玩家广播消息
        public void Broadcast(UserToken exclude_token, ActionCode action_code, string data)
        {
            foreach (UserToken player in players_list)
            {
                if (player == exclude_token) continue;
                player.Send(MessageHandler.PackData(action_code, data));
            }
        }

        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }
        private void RunTimer()
        {
            Thread.Sleep(1000);
            for (int i = 3; i > 0; i--)
            {
                Broadcast(null, ActionCode.STARTTIMER, i.ToString()); // 每秒向服务器发送
                Thread.Sleep(1000);
            }
            Broadcast(null, ActionCode.STARTPLAY, "StartPlay");
        }

        public string Damage(UserToken token, int damage)
        {
            if (host == token)  normal_hp -= damage;
            else host_hp -= damage;
            if (host_hp <= 0)
                return "HostDie";
            else if (normal_hp <= 0)
                return "NormalDie";
            else
                return "NoDie";
        }
    }

    public enum RoomState
    {
        WAIT,
        FULL,
        BATTLE,
        END
    }
}