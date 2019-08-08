using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public UserInfo user_info; // 当前用户的数据
    private Dictionary<RoleType, RoleData> role_data_dict = new Dictionary<RoleType, RoleData>();
    private Transform role_positions;
    public RoleType my_role_type;
    private GameObject my_role_gameobject;
    private GameObject other_role_gameobject;
    private GameObject player_sync_request;
    private ShootRequest shoot_request;
    private DamageRequest damage_request;

    public PlayerManager() { InitRoleDataDict(); }

    private void InitRoleDataDict()
    {
        role_positions = GameObject.Find("RolePositions").transform;
        role_data_dict.Add(RoleType.BLUE, new RoleData(RoleType.BLUE, "Hunter_BLUE", "Arrow_BLUE", role_positions.Find("BluePosition")));
        role_data_dict.Add(RoleType.RED, new RoleData(RoleType.RED, "Hunter_RED", "Arrow_RED", role_positions.Find("RedPosition")));
    }

    private void SpawnAllRoles()
    {
        Debug.LogWarning("创建角色方法");
        foreach (RoleData data in role_data_dict.Values)
        {
            GameObject go = GameObject.Instantiate(data.role_prefab, data.spawn_position, Quaternion.identity);
            go.tag = "Player";
            if (data.role_type == my_role_type)
                my_role_gameobject = go;
            else
                other_role_gameobject = go;
        }
    }

    public GameObject GetMyRoleGameObject()
    {
        return my_role_gameobject;
    }

    public void EnterPlaying()
    {
        SpawnAllRoles(); // 生成角色
        Root.Instance.camera_manager.FollowTarget(); // 相机跟随角色
    }

    public void AddControlScript()
    {
        PlayerMove player_move = my_role_gameobject.AddComponent<PlayerMove>();
        PlayerAttack player_attack = my_role_gameobject.AddComponent<PlayerAttack>();
        player_attack.player_manager = this;
        RoleType role_type = my_role_gameobject.GetComponent<PlayerInfo>().role_type;
        RoleData role_data = null;
        role_data_dict.TryGetValue(role_type, out role_data);
        player_attack.arrow_prefab = role_data.arrow_prefab;
    }

    public void CreateSyncRequest()
    {
        player_sync_request = new GameObject("PlayerSyncRequest");
        player_sync_request.AddComponent<MoveRequest>().local_player_transform = my_role_gameobject.transform;
        player_sync_request.GetComponent<MoveRequest>().local_player_move = my_role_gameobject.GetComponent<PlayerMove>();
        player_sync_request.GetComponent<MoveRequest>().remote_player_transform = other_role_gameobject.GetComponent<Transform>();
        player_sync_request.GetComponent<MoveRequest>().remote_player_animator = other_role_gameobject.GetComponent<Animator>();
        shoot_request = player_sync_request.AddComponent<ShootRequest>();
        damage_request = player_sync_request.AddComponent<DamageRequest>();
        shoot_request.player_manager = this;
    }

    public void Shoot(GameObject arrow_prefab, Vector3 position, Quaternion rotation)
    {
        Root.Instance.audio_manager.PlayNormalAudioClip(AudioManager.TIMER);
        GameObject.Instantiate(arrow_prefab, position, rotation).GetComponent<Arrow>().is_local = true;
        shoot_request.SendRequest(arrow_prefab.GetComponent<Arrow>().type, position, rotation.eulerAngles);
    }
    public void RemoteShoot(RoleType role_type, Vector3 position, Vector3 rotation)
    {
        GameObject arrow_prefab;
        RoleData role_data = null;
        role_data_dict.TryGetValue(role_type, out role_data);
        arrow_prefab = role_data.arrow_prefab;
        other_role_gameobject.GetComponent<Animator>().SetTrigger("Attack");
        Root.Instance.audio_manager.PlayNormalAudioClip(AudioManager.TIMER);
        Transform transform = GameObject.Instantiate(arrow_prefab).GetComponent<Transform>();
        transform.position = position;
        transform.eulerAngles = rotation;
    }

    public void SendDamage(int damage)
    {
        damage_request.SendRequest(damage); // 发起伤害请求
    }

    public void GameOver()
    {
        GameObject.Destroy(my_role_gameobject);
        GameObject.Destroy(other_role_gameobject);
        GameObject.Destroy(player_sync_request);
        shoot_request = null;
        damage_request = null;
    }
}
