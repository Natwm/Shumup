using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenBehaviours : MonoBehaviour, IDamagable
{

    [Space]
    [Header("Token Status")]
    [SerializeField] protected int speedMouvement;
    [SerializeField] private int health;
    [SerializeField] protected float m_FireRate;

    [Space]
    [Header("Token Flag")]
    [SerializeField] protected bool m_CanShoot;
    [SerializeField] protected bool m_CanBeHit = true;

    [Space]
    [Header("Prefabs")]
    [SerializeField] protected GameObject m_bullet;

    protected Timer m_FireTimer;

    [Space]
    [Header("Sound Fmod Action")]
    protected FMOD.Studio.EventInstance deathEffect;
    [SerializeField] protected FMODUnity.EventReference deathSound;

    protected FMOD.Studio.EventInstance hitEffect;
    [SerializeField] protected FMODUnity.EventReference hitSound;

    protected FMOD.Studio.EventInstance basicAttaqueEffect;
    [SerializeField] protected FMODUnity.EventReference basicAttaqueSound;

    protected FMOD.Studio.EventInstance spawnEffect;
    [SerializeField] protected FMODUnity.EventReference spawnSound;

    protected FMOD.Studio.EventInstance deplacementEffect;
    [SerializeField] protected FMODUnity.EventReference deplacementSound;

    public int Health { get => health; set => health = value; }

    private void Start()
    {
        SetUpFmod();
    }

    private void Update()
    {
   
    }

    public void SetUpFmod()
    {
        deathEffect = FMODUnity.RuntimeManager.CreateInstance(deathSound);
        hitEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
        basicAttaqueEffect = FMODUnity.RuntimeManager.CreateInstance(basicAttaqueSound);
        spawnEffect = FMODUnity.RuntimeManager.CreateInstance(spawnSound);
        hitEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
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
        deathEffect.start();
        Destroy(this.gameObject,1.5f);
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    public virtual void TakeDamage(int _AmountOfDamage)
    {
        if (m_CanBeHit)
        {
            Health -= _AmountOfDamage;
            hitEffect.start();

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
        }
    }
}
