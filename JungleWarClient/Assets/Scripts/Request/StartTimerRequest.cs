using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimerRequest : BaseRequest
{
    private void Start()
    {
        InitRequest();
    }

    public override void HandleResponse(string data)
    {
        int time = int.Parse(data);
        GamePanel game_panel = GetComponent<GamePanel>();
        game_panel.ShowTime(time);
    }

    public override void SendRequest()
    {
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.GAME;
        action_code = ActionCode.STARTTIMER;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
