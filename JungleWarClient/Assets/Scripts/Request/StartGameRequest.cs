using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameRequest : BaseRequest
{
    private void Start()
    {
        InitRequest();
    }
    public override void HandleResponse(string data)
    {
        if (data == "StartGame")
        { // 开始游戏
            Root.Instance.ui_manager.PushPanel(UIPanelName.GamePanel);
            Root.Instance.player_manager.EnterPlaying(); // 进入游戏
        }
        else if(data == "NoEnough")
        {
            Root.Instance.ui_manager.ShowMessage("人数不足 无法开始游戏");
        }
        else
        {
            Root.Instance.ui_manager.ShowMessage("无法开始游戏 你不是房主或者房间已不存在");
        }
    }

    public override void SendRequest()
    {
        base.SendRequest("StartGame"); // 发送开始游戏请求
    }

    protected override void InitRequest()
    {
        request_code = RequestCode.GAME;
        action_code = ActionCode.STARTGAME;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
