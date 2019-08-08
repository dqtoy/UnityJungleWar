using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // 相机要跟随的目标
    private Vector3 offset = new Vector3(0, 13, -9);
    private float smoothing = 2;

    private void Update()
    {
        Vector3 target_position = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, target_position, smoothing * Time.deltaTime);
        transform.LookAt(target);
    }
}
