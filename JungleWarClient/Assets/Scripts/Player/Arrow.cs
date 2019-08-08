using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int speed = 20;
    public RoleType type;
    public GameObject explosion_effect;
    public bool is_local = false;

    private void Start()
    {
        Invoke("DestroyMyself", 5f);
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Root.Instance.audio_manager.PlayNormalAudioClip(AudioManager.MISS);
            if (is_local)
            { // 伤害到对方
                Root.Instance.player_manager.SendDamage(Random.Range(10, 20));

            }
            else
            {
                Root.Instance.audio_manager.PlayNormalAudioClip(AudioManager.SHOOT_PERSON);
            }
            GameObject.Instantiate(explosion_effect, transform.position, transform.rotation);
            GameObject.Destroy(this.gameObject);
        }
    }

    private void DestroyMyself()
    {
        Destroy(this.gameObject);
    }
}

