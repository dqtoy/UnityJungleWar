using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler
{
    private ByteArray buffer = new ByteArray(2048); // 数据缓存区

    /// <summary>
    /// 将接收到的数据添加到缓存区
    /// </summary>
    /// <param name="data"></param>
    public void AddReceiveData(byte[] data)
    {
        buffer.AddLast(data);
    }

    /// <summary>
    /// 返回一个消息体
    /// </summary>
    /// <returns></returns>
    public byte[] GetMessageBody()
    {
        if (buffer.GetOffset() < 4) return null;
        int length = BitConverter.ToInt32(buffer.PeekRange(4), 0);
        if (length + 4 > buffer.GetOffset()) return null;
        buffer.PopRange(4);
        byte[] body = buffer.PopRange(length);
        return body;
    }
}
