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

    protected Timer m_FireTimer;

    private void Start()
    {
        
    }

    private void Update()
    {
   
    }

    #region Shoot Logics

    protected void ShootProjectile()
    {
        ShootDisable();
        m_FireTimer.ResetPlay();
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
        m_Health -=_AmountOfDamage;

        if (IsDead())
            Dead();
    }
    #endregion
}
