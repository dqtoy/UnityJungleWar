using System.Collections.Generic;
using System;
using JungleWarServer.NetFrame;
using JungleWarServer.Protocol;

namespace JungleWarServer.Handler
{
    public class GameHandler : BaseHandler
    {
        private HandlerCenter center = null;
        public GameHandler(HandlerCenter center)
        {
            request_code = RequestCode.GAME;
            this.center = center;
        }

        public override string Handle(UserToken token, ActionCode action_code, string str)
        {
            string result = null;
            switch (action_code)
            {
                case ActionCode.STARTGAME:
                    result = StartGame(token, str);
                    return result;
                case ActionCode.MOVE:
                    result = Move(token, str);
                    return result;
                case ActionCode.SHOOT:
                    result = Shoot(token, str);
                    return result;
                case ActionCode.DAMAGE:
                    result = Damage(token, str);
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
        }

        private string StartGame(UserToken token, string str)
        {
            // 判断token是否是房主
            BaseHandler handler = null;
            center.handler_dict.TryGetValue(RequestCode.ROOM, out handler);
            RoomHandler room_handler = handler as RoomHandler;
            Room room = room_handler.GetRoom(token); // 该玩家所在的房间
            if (room != null && room.host == token)
            { // 房主发送的请求 通知房间内的其他玩家可以开始游戏了
                if (room.players_list.Count < 2) return "NotEnough";
                room.Broadcast(token, ActionCode.STARTGAME, "StartGame");
                room.StartTimer();
                return "StartGame"; // 通知房主开始游戏
            }
            else if (room != null && room.host != token)
            { // 不是房主发送的请求
                return "Fail";
            }
            else
            {
                return "Fail";
            }
        }

        private string Move(UserToken token, string data)
        {
            BaseHandler handler = null;
            center.handler_dict.TryGetValue(RequestCode.ROOM, out handler);
            RoomHandler room_handler = handler as RoomHandler;
            Room room = room_handler.GetRoom(token); // 获取玩家所在房间
            if (room != null)
            {
                room.Broadcast(token, ActionCode.MOVE, data);
            }
            return null; // 当前客户端不需要响应
        }

        private string Shoot(UserToken token, string data)
        {
            BaseHandler handler = null;
            center.handler_dict.TryGetValue(RequestCode.ROOM, out handler);
            RoomHandler room_handler = handler as RoomHandler;
            Room room = room_handler.GetRoom(token); // 获取玩家所在房间
            if (room != null)
            {
                room.Broadcast(token, ActionCode.SHOOT, data);
            }
            return null; // 当前客户端不需要响
        }

        private string Damage(UserToken token, string data)
        {
            int damage = int.Parse(data);
            BaseHandler handler = null;
            center.handler_dict.TryGetValue(RequestCode.ROOM, out handler);
            RoomHandler room_handler = handler as RoomHandler;
            Room room = room_handler.GetRoom(token); // 获取玩家所在房间
            string damage_result = room.Damage(token, damage);

            // debug
            Console.WriteLine("攻击请求发生 攻击对象为 " + token.TokenId);
            Console.WriteLine("造成的伤害是 " + damage);
            Console.WriteLine("房间内的血量分别为 " + room.host_hp + " " + room.normal_hp);

            if (damage_result == "HostDie")
            { // 房主死亡
                Console.WriteLine("房主死亡");
                room.Broadcast(room.players_list[1], ActionCode.GAMEOVER, "Fail"); // 通知房主输了
                room.Broadcast(room.players_list[0], ActionCode.GAMEOVER, "Success");
                Room temp = null;
                room_handler.hostid_2_room.Remove(room.players_list[0].TokenId, out temp);
                return null;
            }
            else if (damage_result == "NormalDie")
            { // 普通玩家死亡
                Console.WriteLine("普通死亡");
                room.Broadcast(room.players_list[1], ActionCode.GAMEOVER, "Success");
                room.Broadcast(room.players_list[0], ActionCode.GAMEOVER, "Fail");
                Room temp = null;
                room_handler.hostid_2_room.Remove(room.players_list[0].TokenId, out temp);
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}