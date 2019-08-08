using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRequest : BaseRequest
{
    public PlayerManager player_manager;
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
        Debug.Log("进入到Shoot的响应方法");
        Debug.Log(data);
        string[] datas = data.Split('|');
        RoleType type = (RoleType)int.Parse(datas[0]);
        Vector3 position = Parse(datas[1]);
        Vector3 rotation = Parse(datas[2]);
        player_manager.RemoteShoot(type, position, rotation);
    }

    private Vector3 Parse(string str)
    {
        string[] strs = str.Split(',');
        float x = float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);
        return new Vector3(x, y, z);
    }

    public override void SendRequest()
    {
    }

    public void SendRequest(RoleType role_type, Vector3 position, Vector3 rotation)
    {
        InitRequest();
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)role_type, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z);
        base.SendRequest(data);
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.GAME;
        action_code = ActionCode.SHOOT;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
