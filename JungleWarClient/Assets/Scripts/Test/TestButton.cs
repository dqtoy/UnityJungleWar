using System.Linq;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public Net net;
    private static int count = 0;

    private void Awake()
    {
        net.RegisterHandleEvents(TestHandle);
    }

    public void OnButtonClick()
    {
        byte[] request_bytes = BitConverter.GetBytes((int)RequestCode.USER);
        byte[] action_bytes = BitConverter.GetBytes((int)ActionCode.REGISTER);
        byte[] str_bytes = Encoding.UTF8.GetBytes("username,password");
        byte[] length_bytes = BitConverter.GetBytes(request_bytes.Length + action_bytes.Length + str_bytes.Length);
        byte[] result = length_bytes.Concat(request_bytes).Concat(action_bytes).Concat(str_bytes).ToArray();
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                net.Send(result);
            }
        }
    }

    public void TestHandle(byte[] data)
    {
        count++;
        string str = Encoding.UTF8.GetString(data);
        Debug.Log("服务器发送的数据为 " + str);
        
        Debug.Log("次数为 " + count);
    }
}
