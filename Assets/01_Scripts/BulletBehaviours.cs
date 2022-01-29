using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviours : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Vector3 m_MoveDirection;
    [SerializeField] private float m_Speed;

    public Vector3 MoveDirection { get => m_MoveDirection; set => m_MoveDirection = value; }
    public float Speed { get => m_Speed; set => m_Speed = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchBullet();
    }


    public void LaunchBullet()
    {
        rb.velocity = MoveDirection * Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.contacts[0].normal);
        MoveDirection = collision.contacts[0].normal;
        LaunchBullet();
    }
}
