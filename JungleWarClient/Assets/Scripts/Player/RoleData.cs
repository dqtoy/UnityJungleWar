using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleData
{
    public RoleType role_type;
    public GameObject role_prefab;
    public GameObject arrow_prefab;
    public Vector3 spawn_position;

    public RoleData(RoleType type, string role_path, string arrow_path, Transform spawn_position)
    {
        this.role_type = type;
        this.role_prefab = Resources.Load("Prefabs/" + role_path) as GameObject;
        this.arrow_prefab = Resources.Load("Prefabs/" + arrow_path) as GameObject;
        this.spawn_position = spawn_position.position;
    }
}
