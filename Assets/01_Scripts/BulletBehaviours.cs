using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviours : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Vector3 m_MoveDirection;
    [SerializeField] private float m_Speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchBullet();
    }


    public void LaunchBullet()
    {
        rb.velocity = m_MoveDirection * m_Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.contacts[0].normal);
        m_MoveDirection = collision.contacts[0].normal;
        LaunchBullet();
    }
}
