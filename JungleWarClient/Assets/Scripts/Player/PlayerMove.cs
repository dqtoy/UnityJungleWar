using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float speed = 5;
    private Animator animator = null;
    public float forward;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") == false) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
            float res = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
            forward = res;
            animator.SetFloat("Forward", res);
        }
    }
}
