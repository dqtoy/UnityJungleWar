using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator = null;
    public GameObject arrow_prefab;
    private Transform left_hand_transform;
    private Vector3 shoot_dir;

    public PlayerManager player_manager = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        left_hand_transform = transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");
    }   

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool is_collider = Physics.Raycast(ray, out hit);
                if (is_collider)
                {
                    Vector3 target_point = hit.point;
                    target_point.y = transform.position.y;
                    shoot_dir = target_point - transform.position;
                    transform.rotation = Quaternion.LookRotation(shoot_dir);
                    animator.SetTrigger("Attack");
                    Invoke("Shoot", 0.3f);
                }
            }
        }
    }

    private void Shoot()
    {
        player_manager.Shoot(arrow_prefab, left_hand_transform.position, Quaternion.LookRotation(shoot_dir));
    }
}
