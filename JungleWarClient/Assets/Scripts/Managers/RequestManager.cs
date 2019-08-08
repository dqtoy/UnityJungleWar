using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager
{
    // 请求字典 存储Request
    private Dictionary<ActionCode, BaseRequest> request_dict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode action_code, BaseRequest request)
    {
        if(request_dict.ContainsKey(action_code))
        {
            Debug.LogWarning("重复添加request 添加的键为:" + Enum.GetName(typeof(ActionCode), action_code));
            request_dict.Remove(action_code);
            request_dict.Add(action_code, request);
            return;
        }
        request_dict.Add(action_code, request);
    }

    public BaseRequest GetRequest(ActionCode action_code)
    {
        BaseRequest base_request = null;
        bool is_get = request_dict.TryGetValue(action_code, out base_request);
        if(is_get) return base_request;
        return null;
    }

    /// <summary>
    /// 处理服务器的响应
    /// </summary>
    /// <param name="action_code"></param>
    /// <param name="data"></param>
    public void HandleResponse(ActionCode action_code, string data)
    {
        // Debug.Log("服务器发送的数据如下:");
        // Debug.Log("ActionCode " + Enum.GetName(typeof(ActionCode), action_code));
        // Debug.Log("Data " + data);
        BaseRequest request = null;
        request_dict.TryGetValue(action_code, out request);
        request.HandleResponse(data);
    }
}
