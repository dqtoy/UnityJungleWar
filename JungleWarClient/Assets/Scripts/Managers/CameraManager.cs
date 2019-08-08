using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager
{
    private GameObject camera_go;
    private Animator camera_anim;
    private FollowTarget follow_target;
    private Vector3 original_position;
    private Vector3 original_rotation;

    public CameraManager() { Init(); }

    public void FollowTarget()
    {
        follow_target.target = Root.Instance.player_manager.GetMyRoleGameObject().transform;
        camera_anim.enabled = false;
        original_position = camera_go.transform.position;
        original_rotation = camera_go.transform.eulerAngles;
        Quaternion target_quaternion = Quaternion.LookRotation(follow_target.target.position - camera_go.transform.position);
        camera_go.transform.DORotateQuaternion(target_quaternion, 0.3f).OnComplete(delegate ()
        {
            follow_target.enabled = true;
        });
    }
    public void WalkThrougnScene()
    {
        follow_target.enabled = false;
        camera_go.transform.DOMove(original_position, 1f);
        camera_go.transform.DORotate(original_rotation, 1f).OnComplete(delegate ()
        {
            camera_anim.enabled = true;
        });
    }

    private void Init()
    {
        camera_go = Camera.main.gameObject;
        camera_anim = camera_go.GetComponent<Animator>();
        follow_target = camera_go.GetComponent<FollowTarget>();
        Debug.Log("原始位置 " + original_position);
        Debug.Log("原始旋转 " + original_rotation);
    }
}
