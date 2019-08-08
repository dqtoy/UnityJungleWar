using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomRequest : BaseRequest
{
    public override void HandleResponse(string data)
    {
        string[] datas = data.Split(',');
        if (datas[0] == "Success")
        { // 加入房间成功 弹出房间列表
            Root.Instance.ui_manager.PushPanel(UIPanelName.RoomPanel);
            if (datas[1] == "RED") Root.Instance.player_manager.my_role_type = RoleType.RED;
        }
        else if (data == "NoThisRoom")
        {
            Root.Instance.ui_manager.ShowMessage("该房间已不存在 请刷新");
        }
        // if(data.Substring(0, 7) != "Success")
        // {
        //     GetComponent<RoomListPanel>().JoinRoomResponse(data, null, null);
        //     return;
        // }
        // UserInfo info1 = null;
        // UserInfo info2 = null;
        // string[] datas = data.Split('|');
        // if(datas[0] == "Success")
        // {
        //     info1 = new UserInfo(datas[1]);
        //     info2 = new UserInfo(datas[2]);
        // }
        // GetComponent<RoomListPanel>().JoinRoomResponse(data, info1, info2);
    }

    public override void SendRequest()
    {
        InitRequest();
    }

    public void SendRequest(int room_id)
    {
        InitRequest();
        string data = room_id.ToString();
        base.SendRequest(data);
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.ROOM;
        action_code = ActionCode.JOINROOM;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
