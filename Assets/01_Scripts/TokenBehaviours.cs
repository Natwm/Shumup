using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenBehaviours : MonoBehaviour, IDamagable
{

    [Space]
    [Header("Token Status")]
    [SerializeField] protected int speedMouvement;
    [SerializeField] protected int m_Health;
    [SerializeField] protected float m_FireRate;

    [Space]
    [Header("Token Flag")]
    [SerializeField] protected bool m_CanShoot;
    [SerializeField] protected bool m_CanBeHit = true;

    [Space]
    [Header("Prefabs")]
    [SerializeField] protected GameObject m_bullet;

    protected Timer m_FireTimer;

    private void Start()
    {
        
    }

    private void Update()
    {
   
    }

    #region Shoot Logics

    public void ShootProjectile()
    {
        ShootDisable();
        m_FireTimer.ResetPlay();
        Shoot(Vector3.up);

    }

    protected void Shoot(Vector3 direction)
    {
        GameObject bullet = Instantiate(m_bullet,transform.position, Quaternion.identity);
        bullet.GetComponent<BulletBehaviours>().MoveDirection = direction;
        bullet.GetComponent<BulletBehaviours>().Speed = 2f;

        bullet.GetComponent<BulletBehaviours>().LaunchBullet();
    }

    #region Shoot Flag
    protected void ShootEnable()
    {
        m_CanShoot = true;
    }

    protected void ShootDisable()
    {
        m_CanShoot = false;
    }
    #endregion

    #endregion


    #region Interface
    public void Dead()
    {
        Destroy(this.gameObject);
    }

    public bool IsDead()
    {
        return m_Health <= 0;
    }

    public void TakeDamage(int _AmountOfDamage)
    {
        if (m_CanBeHit)
        {
            m_Health -= _AmountOfDamage;

            if (IsDead())
                Dead();
        }
        
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        BulletBehaviours hit;
        if (collision.gameObject.TryGetComponent<BulletBehaviours>(out hit))
        {
            TakeDamage(hit.GetAmountOfDamage());
            Destroy(hit.gameObject);
        }
    }
}
