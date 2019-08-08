using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverRequest : BaseRequest
{
    private void Start()
    {
        InitRequest();
    }

    public override void HandleResponse(string data)
    {
        GetComponent<GamePanel>().OnGameOverResponse(data);
    }

    public override void SendRequest()
    {
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.GAME;
        action_code = ActionCode.GAMEOVER;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
