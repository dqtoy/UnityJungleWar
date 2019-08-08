using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRoomRequest : BaseRequest
{
    private void Awake()
    {
    }

    public override void HandleResponse(string data)
    {
        // debug
        Debug.Log("UpdateRoom接收到的数据为" + data);
        UserInfo info1 = null;
        UserInfo info2 = null;
        string[] datas = data.Split('|');
        info1 = new UserInfo(datas[0]);
        if (datas.Length > 1)
            info2 = new UserInfo(datas[1]);
        GetComponent<RoomPanel>().SetHostPlayerInfo(info1.usrename, info1.totalcount.ToString(), info1.wincount.ToString());
        if (info2 != null)
            GetComponent<RoomPanel>().SetNotHostPlayerInfo(info2.usrename, info2.totalcount.ToString(), info2.wincount.ToString());
        else
            GetComponent<RoomPanel>().ClearNotHostPlayerInfo();
    }

    public override void SendRequest()
    {
        Debug.Log("跟新房间的发送数据");
        InitRequest();
        base.SendRequest("UpdateRoom");
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.ROOM;
        action_code = ActionCode.UPDATEROOM;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
