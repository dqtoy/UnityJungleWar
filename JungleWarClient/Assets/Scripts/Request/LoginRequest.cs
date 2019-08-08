using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    private void Start()
    {
    }

    protected override void InitRequest()
    {
        if (request_code == RequestCode.NONE)
        {
            request_code = RequestCode.USER;
            action_code = ActionCode.LOGIN;
            Root.Instance.request_manager.AddRequest(action_code, this);
        }
    }

    public void SendRequest(string username, string password)
    {
        InitRequest();
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void HandleResponse(string data)
    {
        if (data == "Fail")
        { // 登录失败
            Root.Instance.ui_manager.ShowMessage("登录失败");
        }
        else
        {
            string[] strs = data.Split(',');
            if (strs[0] == "Success")
            { // 登录成功
                string username = strs[1];
                int totalcount = int.Parse(strs[2]);
                int wincount = int.Parse(strs[3]);
                UserInfo userinfo = new UserInfo(username, totalcount, wincount);
                Root.Instance.player_manager.user_info = userinfo; // 登录成功后将数据设置到PlayerManager中
                Root.Instance.ui_manager.PushPanel(UIPanelName.RoomListPanel);
            }
            else
            { // 我也不知道怎么回事
                Root.Instance.ui_manager.ShowMessage("不知道啊发生了什么！");
            }
        }
    }

    public override void SendRequest()
    {
    }
}
