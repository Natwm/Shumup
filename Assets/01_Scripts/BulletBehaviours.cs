using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviours : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Vector3 m_MoveDirection;
    [SerializeField] private float m_Speed;

    [Space]
    [Header("Sound Fmod Action")]
    protected FMOD.Studio.EventInstance rebondMurEffect;
    [SerializeField] protected FMODUnity.EventReference rebondMurSound;

    protected FMOD.Studio.EventInstance rebondEnemyEffect;
    [SerializeField] protected FMODUnity.EventReference rebondEnemySound;

    protected FMOD.Studio.EventInstance rebondPlayerEffect;
    [SerializeField] protected FMODUnity.EventReference rebondPlayerSound;

    public Vector3 MoveDirection { get => m_MoveDirection; set => m_MoveDirection = value; }
    public float Speed { get => m_Speed; set => m_Speed = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //LaunchBullet();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            LaunchBullet();
    }

    public virtual void SetUpFmod()
    {
        rebondPlayerEffect = FMODUnity.RuntimeManager.CreateInstance(rebondPlayerSound);
        rebondEnemyEffect = FMODUnity.RuntimeManager.CreateInstance(rebondEnemySound);
        rebondMurEffect = FMODUnity.RuntimeManager.CreateInstance(rebondMurSound);
    }

    public void LaunchBullet()
    {
        GetComponent<Rigidbody>().velocity = MoveDirection * Speed;
    }

    public void LaunchBullet(Vector3 direction, bool isPerfectShoot)
    {
        Speed = isPerfectShoot ? Speed * 1.2f : Speed * 1.35f;
        GetComponent<Rigidbody>().velocity = direction * Speed;
    }

    public void LaunchBullet(Vector3 direction, float speed)
    {
        this.Speed = speed;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    public int GetAmountOfDamage()
    {
        return 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        MoveDirection = collision.contacts[0].normal;
        LaunchBullet();
        if (collision.gameObject.CompareTag("Wall"))
        {
            rebondMurEffect.start();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            rebondPlayerEffect.start();
        }
        else
        {
            rebondEnemyEffect.start();
        }
    }
}
