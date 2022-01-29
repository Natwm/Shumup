using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletBehaviours bullet;
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.TryGetComponent<BulletBehaviours>(out bullet))
        {
            print(collision.contacts[0].normal);
            bullet.LaunchBullet(-1*(collision.contacts[0].normal),10);
        }
    }
}
