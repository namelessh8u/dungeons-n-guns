using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        //direction = PlayerController.instance.transform.position - transform.position;
        // direction.Normalize();
        direction = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (!BossController.instance.gameObject.activeInHierarchy) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.instance.PlaySFX(4);
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlyaer();
        }
        Destroy(gameObject);
    }
}
