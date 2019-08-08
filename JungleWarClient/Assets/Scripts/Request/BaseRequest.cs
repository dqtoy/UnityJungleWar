using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRequest : MonoBehaviour
{
    protected RequestCode request_code = RequestCode.NONE;
    protected ActionCode action_code = ActionCode.NONE;

    protected abstract void InitRequest();

    public abstract void SendRequest();

    public void SendRequest(string data)
    {
        Root.Instance.SendRequest(request_code, action_code, data);
    }

    /// <summary>
    /// 处理响应
    /// </summary>
    public abstract void HandleResponse(string data);
}
