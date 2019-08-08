using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListRoomRequest : BaseRequest
{

    public override void HandleResponse(string data)
    {
        if (data == "NoRoom")
        {
            GetComponent<RoomListPanel>().LoadRoomItem(null);
            return;
        }
       string[] roominfo_array = data.Split('|');
        // debug
        Debug.Log("接收到的房间数量为 " + roominfo_array.Length);
        List<UserInfo> userinfo_list = new List<UserInfo>();
        foreach (string roominfo in roominfo_array)
        {
            // debug
            Debug.Log("一个房间信息为 " + roominfo);
            string[] strs = roominfo.Split(',');
            Debug.Log("切割出的字符串为" + strs[0] + "," + strs[1] + "," + strs[2] + "," + strs[3]);
            UserInfo info = new UserInfo(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3]));
            userinfo_list.Add(info);
        }
        if (userinfo_list.Count > 0)
            GetComponent<RoomListPanel>().LoadRoomItem(userinfo_list);
    }

    public override void SendRequest()
    {
        InitRequest();
        base.SendRequest("list_room");
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.ROOM;
        action_code = ActionCode.LISTROOM;
        Root.Instance.request_manager.AddRequest(ActionCode.LISTROOM, this);
    }
}
