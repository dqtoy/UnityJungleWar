using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRequest : BaseRequest
{
    protected override void InitRequest()
    {
        if(request_code != RequestCode.NONE) return;
        request_code = RequestCode.USER;
        action_code = ActionCode.REGISTER;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }

    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void HandleResponse(string data)
    {
        if (data == "Success")
        { // 注册成功
            Root.Instance.ui_manager.ShowMessage("注册成功");
        }
        else if (data == "Fail")
        {
            Root.Instance.ui_manager.ShowMessage("注册失败");
        }
        else
        {
            Root.Instance.ui_manager.ShowMessage("我也不知道注册成功没有");
        }
    }

    public override void SendRequest()
    {
    }
}
