using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomRequest : BaseRequest
{
    protected override void InitRequest()
    {
        if (request_code == RequestCode.NONE || action_code == ActionCode.NONE)
        {
            request_code = RequestCode.ROOM;
            action_code = ActionCode.CREATEROOM;
            Root.Instance.request_manager.AddRequest(ActionCode.CREATEROOM, this);
        }
    }

    public override void HandleResponse(string data)
    {
        string[] datas = data.Split(',');
        if (datas[0] == "Success")
        { // 创建成功
            if (datas[1] == "BLUE") // 设置玩家控制的角色颜色
                Root.Instance.player_manager.my_role_type = RoleType.BLUE;
            // UserInfo info = Root.Instance.player_manager.user_info; // 用户信息
            Root.Instance.ui_manager.PushPanel(UIPanelName.RoomPanel); // 创建成功 显示RoomPanel
            // RoomPanel room_panel = (RoomPanel)Root.Instance.ui_manager.GetPanel(UIPanelName.RoomPanel);
            // 设置房间信息
            // room_panel.SetHostPlayerInfo(info.usrename, info.totalcount.ToString(), info.wincount.ToString());
            // room_panel.ClearNotHostPlayerInfo();
        }
        else
        {
            Root.Instance.ui_manager.ShowMessage("创建房间失败");
        }
    }

    public override void SendRequest()
    {
        InitRequest();
        base.SendRequest("create_room");
    }
}
