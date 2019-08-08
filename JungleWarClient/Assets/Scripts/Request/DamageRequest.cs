using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRequest : BaseRequest
{
    private void Awake()
    {
        InitRequest();
    }
    private void Start()
    {
        InitRequest();
    }
    public override void HandleResponse(string data)
    {
    }

    public override void SendRequest()
    {
    }

    public void SendRequest(int damage)
    {
        base.SendRequest(damage.ToString());
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.GAME;
        action_code = ActionCode.DAMAGE;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
