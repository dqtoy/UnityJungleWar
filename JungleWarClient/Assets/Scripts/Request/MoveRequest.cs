using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRequest : BaseRequest
{
    public Transform local_player_transform;
    public PlayerMove local_player_move;

    public Transform remote_player_transform;
    public Animator remote_player_animator;

    private int sync_rate = 40;

    private void Awake()
    {
        InitRequest();
    }

    private void Start()
    {
        InitRequest();
        InvokeRepeating("SyncLocalPlayer", 0, 1f / sync_rate);
    }

    public override void HandleResponse(string data)
    {
        string[] datas = data.Split('|');
        Vector3 position = Parse(datas[0]);
        Vector3 rotation = Parse(datas[1]);
        float forward = float.Parse(datas[2]);
        SyncRemotePlayer(position, rotation, forward);
    }
    private Vector3 Parse(string str)
    {
        string[] strs = str.Split(',');
        float x = float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);
        return new Vector3(x, y, z);
    }

    private void SyncLocalPlayer()
    {
        SendRequest(local_player_transform.position.x, local_player_transform.position.y, local_player_transform.position.z,
                    local_player_transform.eulerAngles.x, local_player_transform.eulerAngles.y, local_player_transform.eulerAngles.z,
                    local_player_move.forward);
    }

    private void SyncRemotePlayer(Vector3 position, Vector3 rotation, float forward)
    {
        remote_player_transform.position = position;
        remote_player_transform.eulerAngles = rotation;
        remote_player_animator.SetFloat("Forward", forward);
    }

    public override void SendRequest()
    {
    }

    private void SendRequest(float position_x, float position_y, float position_z, float rotation_x, float rotation_y, float rotation_z, float forward)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", position_x, position_y, position_z, rotation_x, rotation_y, rotation_z, forward);
        base.SendRequest(data);
    }

    protected override void InitRequest()
    {
        if (request_code != RequestCode.NONE) return;
        request_code = RequestCode.GAME;
        action_code = ActionCode.MOVE;
        Root.Instance.request_manager.AddRequest(action_code, this);
    }
}
