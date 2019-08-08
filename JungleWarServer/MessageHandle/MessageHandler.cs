using System.Linq;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using JungleWarServer.NetFrame;
using JungleWarServer.Protocol;

namespace JungleWarServer.MessageHandle
{
    public class MessageHandler
    {
        private Dictionary<UserToken, ByteArray> token_buffer_dict = new Dictionary<UserToken, ByteArray>();

        /// <summary>
        /// 添加一个连接对象
        /// </summary>
        /// <param name="token"></param>
        public void AddToken(UserToken token)
        {
            if (token_buffer_dict.ContainsKey(token))
                return; // 已经包含 无需重复添加

            ByteArray array = new ByteArray(2048);
            token_buffer_dict.Add(token, array);
        }
        /// <summary>
        /// 移除一个连接对象
        /// </summary>
        /// <param name="token"></param>
        public void RemoveToken(UserToken token)
        {
            if (token_buffer_dict.ContainsKey(token))
            {
                token_buffer_dict.Remove(token);
            }
        }

        public void AddTokenReceiveData(UserToken token, byte[] data)
        {
            if (!token_buffer_dict.ContainsKey(token))
            { // 字典里没有该连接对象时 首先添加该连接对象与对应的字节数组抽象类
                ByteArray array = new ByteArray(2048);
                token_buffer_dict.Add(token, array);
            }
            // 处理字节数组抽象类里的数据
            ByteArray temp = null;
            token_buffer_dict.TryGetValue(token, out temp);
            temp.AddLast(data); // 将接收到的数据添加到字节数组抽象类里
        }

        /// <summary>
        /// 获取一个完整的消息体
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public byte[] GetMessageBody(UserToken token)
        {
            ByteArray array = null;
            byte[] body = null;
            token_buffer_dict.TryGetValue(token, out array);
            int offset = array.GetOffset();
            if (offset < 4) return null;

            int length = BitConverter.ToInt32(array.PeekRange(4));
            if (length + 4 > offset) return null;

            array.PopRange(4);
            body = array.PopRange(length);
            return body;
        }

        // 从字节数组抽象类中获取一个消息体
        private byte[] GetMessageBody(ByteArray array)
        {
            if (array.GetOffset() <= 4) // 消息体不完整 直接返回null
                return null;

            int length = BitConverter.ToInt32(array.PeekRange(4));
            if (4 + length < array.GetOffset())
                return null;

            array.PopRange(4); // 把长度报文弹出
            byte[] body = array.PopRange(length);
            return body;
        }

        public static byte[] PackData(ActionCode action_code, string data)
        {
            byte[] action_bytes = BitConverter.GetBytes((int)action_code);
            byte[] data_bytes = Encoding.UTF8.GetBytes(data);
            byte[] length_bytes = BitConverter.GetBytes(action_bytes.Length + data_bytes.Length);
            return length_bytes.Concat(action_bytes).Concat(data_bytes).ToArray();
        }
    }
}