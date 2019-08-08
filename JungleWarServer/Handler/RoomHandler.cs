using System.Xml.Linq;
using System.Text;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using JungleWarServer.NetFrame;
using JungleWarServer.Protocol;
using JungleWarServer.DataBase;

namespace JungleWarServer.Handler
{
    public class RoomHandler : BaseHandler
    {
        private HandlerCenter center = null;
        public ConcurrentDictionary<int, Room> hostid_2_room = new ConcurrentDictionary<int, Room>();

        public RoomHandler(HandlerCenter center)
        {
            request_code = RequestCode.ROOM;
            this.center = center;
        }

        public override string Handle(UserToken token, ActionCode action_code, string str)
        {
            string result = null;
            switch (action_code)
            {
                case ActionCode.CREATEROOM:
                    result = CreateRoom(token);
                    TestPrint();
                    return result;
                case ActionCode.LISTROOM:
                    result = ListRoom();
                    TestPrint();
                    return result;
                case ActionCode.JOINROOM:
                    result = JoinRoom(token, str);
                    TestPrint();
                    return result;
                case ActionCode.QUITROOM:
                    result = QuitRoom(token, str);
                    TestPrint();
                    return result;
                case ActionCode.UPDATEROOM:
                    result = UpdateRoom(token, str);
                    TestPrint();
                    return result;
                default:
                    return null;
            }
        }

        public override void OnClientAccept(int token_id, UserToken token)
        {
        }

        public override void OnClientClose(int id, UserToken token, string message)
        {
            Room r = null;
            QuitRoom(token, message);
        }

        private string CreateRoom(UserToken token)
        {
            Room room = new Room();
            room.host = token;
            room.state = RoomState.WAIT;
            room.players_list.Add(token);
            hostid_2_room.TryAdd(token.TokenId, room);

            // debug
            Console.WriteLine("创建房间成功 " + room.host_id);
            return "Success" + "," + "BLUE";
        }

        private string ListRoom()
        {
            if (hostid_2_room.Values.Count <= 0) return "NoRoom"; // 如果没有房间 返回NoRoom字符串

            StringBuilder result = new StringBuilder();
            foreach (Room room in hostid_2_room.Values)
            { // 遍历所有房间
                if (room.state == RoomState.WAIT)
                    result.Append(room.GetHostInfo() + "|");
            }
            if (result.Length > 0)
                result.Remove(result.Length - 1, 1); // 把末尾的 | 移除掉
            else
                result.Append("NoRoom");
            // debug
            Console.WriteLine("ListRoom的数据为 " + result.ToString());
            return result.ToString();
        }

        private string JoinRoom(UserToken token, string data)
        {
            int room_id = int.Parse(data); // 要加入的房间ID
            Room room = null; // 要加入的房间对象
            bool is_get = hostid_2_room.TryGetValue(room_id, out room);
            if (!is_get) return "NoThisRoom";
            if (room.players_list.Count < 2)
            {
                room.players_list.Add(token);
                room.state = RoomState.FULL;
                room.Broadcast(token, ActionCode.UPDATEROOM, UpdateRoom(room.host, "UpdateRoom"));
                return "Success" + "," + "RED";
            }
            else
                return "RoomIsFull";
        }

        public string QuitRoom(UserToken token, string data)
        {
            foreach (Room room in hostid_2_room.Values)
            { // 遍历所有房间
                if (room.players_list.Contains(token))
                { // 找到该玩家所在的房间
                    if (room.host == token)
                    { // 房主离开房间
                        room.Broadcast(token, ActionCode.QUITROOM, "Success");
                        Room r = null;
                        hostid_2_room.Remove(room.host_id, out r); // 移除该房间
                        return "Success";
                    }
                    else
                    { // 普通玩家离开房间
                        room.players_list.Remove(token);
                        if (room.players_list.Count < 2) room.state = RoomState.WAIT;
                        // 通知房主有人离开房间 更新房间画面
                        room.Broadcast(token, ActionCode.UPDATEROOM, UpdateRoom(room.host, "UpdateRoom"));
                        return "Success";
                    }
                }
            }
            return "NoThisRoom";
        }

        private string UpdateRoom(UserToken token, string data)
        {
            foreach (Room room in hostid_2_room.Values)
            {
                if (room.players_list.Contains(token))
                { // 找到了该玩家所在的房间
                    StringBuilder room_info = new StringBuilder();
                    foreach (UserToken player in room.players_list)
                    {
                        UserInfo info = DB.GetUserInfo(player);
                        room_info.Append(token.TokenId + "," + info.username + "," + info.totalcount + "," + info.wincount + "|");
                    }
                    room_info.Remove(room_info.Length - 1, 1);
                    // 返回房间信息的格式 id,username,totalcount,wincount|id,username,totalcount,wincount
                    // debug
                    Console.WriteLine("UpdateRoom的数据为 " + room_info.ToString());
                    return room_info.ToString();
                }
            }
            return "NoThisRoom";
        }

        /// <summary>
        /// 获取该玩家所在的房间
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Room GetRoom(UserToken token)
        {
            foreach (Room room in hostid_2_room.Values)
            {
                if (room.players_list.Contains(token))
                    return room;
            }
            return null;
        }

        // debug
        private void TestPrint()
        {
            Console.WriteLine("-------------输出房间信息如下--------------------------");
            foreach (Room room in hostid_2_room.Values)
            {
                Console.WriteLine("房间的房主ID为 " + room.host_id);
                Console.WriteLine("房间内的人数为 " + room.players_list.Count);
                Console.WriteLine("房间的状态为 " + Enum.GetName(typeof(RoomState), room.state));
                Console.WriteLine("房间内的第一个玩家ID " + room.players_list[0].TokenId);
                if (room.players_list.Count >= 2)
                    Console.WriteLine("房间内第二个玩家ID " + room.players_list[1].TokenId);
            }
            Console.WriteLine("-------------输出房间信息如下--------------------------");
        }
    }
}