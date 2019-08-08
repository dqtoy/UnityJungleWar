using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayRequest : BaseRequest
{
    private void Start()
    {
        InitRequest();
    }

    public override void HandleResponse(string data)
    {
        if (data == "StartPlay")
        {
            Root.Instance.player_manager.AddControlScript(); // 添加控制脚本
            Root.Instance.player_manager.CreateSyncRequest();
        }
    }

    public override void SendRequest()
    {
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.GAME;
        action_code = ActionCode.STARTPLAY;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
