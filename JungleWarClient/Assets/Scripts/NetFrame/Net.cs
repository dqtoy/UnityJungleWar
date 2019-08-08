using System;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

/// <summary>
/// 该类负责与服务器进行通讯
/// </summary>
public class Net : MonoBehaviour
{
    private const string IP = "127.0.0.1";
    private const int PORT = 8899;
    private const int BUFFER_SIZE = 2048;

    private Socket socket = null;
    private byte[] buffer = new byte[BUFFER_SIZE]; // 数据缓存区
    private ConcurrentQueue<byte[]> message_body_queue = new ConcurrentQueue<byte[]>(); // 消息体队列
    private MessageHandler message_handler = new MessageHandler();

    private event Action<byte[]> handle_message_body_events;

    /// <summary>
    /// 注册处理消息体的方法
    /// </summary>
    /// <param name="handle_events"></param>
    public void RegisterHandleEvents(Action<byte[]> handle_events)
    {
        this.handle_message_body_events += handle_events;
    }

    public void Send(byte[] data)
    {
        if (socket == null) return;
        socket.Send(data);
    }

    private void Start()
    {
        Run();
    }

    private void Update()
    {
        ClearQueue();
    }

    /// <summary>
    /// 开始运作
    /// </summary>
    private void Run()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse(IP), PORT));
        BeginReceiveAsync();
    }
    private void BeginReceiveAsync()
    {
        socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallBack, null);
    }
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);
            byte[] data = new byte[count];
            Buffer.BlockCopy(buffer, 0, data, 0, count);
            message_handler.AddReceiveData(data);
            Buffer2Queue();
            BeginReceiveAsync();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            Close();
        }
    }

    // 将MessageHandler里的所有消息体提取出来并添加到消息体队列中
    private void Buffer2Queue()
    {
        byte[] body = null;
        while (true)
        {
            body = message_handler.GetMessageBody();
            if (body == null) return;
            message_body_queue.Enqueue(body);
        }
    }

    // 处理消息体队列中的消息体
    private void ClearQueue()
    {
        byte[] body = null;
        while (message_body_queue.Count > 0)
        {
            message_body_queue.TryDequeue(out body);
            handle_message_body_events(body); // 交由上层处理
        }
    }

    public void Close()
    {
        if (socket != null) socket.Close();
    }
}
