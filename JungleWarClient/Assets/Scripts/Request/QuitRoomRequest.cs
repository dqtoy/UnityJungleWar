using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitRoomRequest : BaseRequest
{
    private void Start()
    {
        InitRequest();
    }

    public override void HandleResponse(string data)
    {
        if (data == "Success")
        { // 退出成功
                Root.Instance.ui_manager.PopPanel(); // 把房间列表弹出
        }
        else
        { // 退出失败
            Debug.Log("退出响应数据为" + data);
            Root.Instance.ui_manager.ShowMessage("房间退出失败");
        }
    }

    // 发起退出房间的请求
    public override void SendRequest()
    {
        InitRequest();
        base.SendRequest("QuitRoom");
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.ROOM;
        action_code = ActionCode.QUITROOM;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
