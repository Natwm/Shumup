using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterBehaviours : TokenBehaviours
{

    public static CharacterBehaviours instance;

    [Space]
    [Header("Player Component")]
    [SerializeField] private CharacterController m_Controller;
    [SerializeField] private BoxCollider m_Collider;

    [Space]
    [Header("Player status")]
    [SerializeField] private float m_DashPower;
    [SerializeField] private float m_DashTimer;
    [SerializeField] private float m_TimeBetweenDash;
    [SerializeField] private float m_TimeBetweenTwoDeflect;

    [Space]
    [Header("Timer")]
    Timer m_DashDuration;
    Timer m_DashWaitDuration;
    Timer m_DeflectWaitDuration;

    [Space]
    [Header("Flag")]
    [SerializeField] private bool m_CanDash;
    [SerializeField] private bool m_CanDeflect;

    [SerializeField] private GameObject orientation;
    [SerializeField] private LayerMask bulletLayer;


    [Space]
    [Header("Perfect Deflect")]
    [SerializeField] private float minDistanceToPerfect;
    [SerializeField] private float maxDistanceToPerfect;

    #region Awake | Start | Update
    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : CharacterScript");
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_FireTimer = new Timer(m_FireRate, ShootEnable);
        m_DashDuration = new Timer(m_DashTimer+0.5f, EnableCollider);
        m_DashWaitDuration = new Timer(m_TimeBetweenDash, DashEnable);
        m_DeflectWaitDuration = new Timer(m_TimeBetweenTwoDeflect, DeflectEnable);
    }

    // Update is called once per frame
    void Update()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift) && m_CanDash)
        {
            DashDisable();
            m_DashWaitDuration.ResetPlay();
            Dash( moveHorizontal,  moveVertical);
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_CanDeflect)
        {
            if (orientation != null)
            {
                m_DeflectWaitDuration.ResetPlay();
                DeflectDisable();
                orientation.GetComponent<shield>().UseDeflect();
            }
                
        }

        PlayerMovement( moveHorizontal, moveVertical);
    }

    #endregion

    public  virtual void ShootProjectile() 
    {
        ShootDisable();
        m_FireTimer.ResetPlay();
        Shoot(RayCastRotation.instance.ShootGO.transform.position);
    }

    protected virtual void Shoot(Vector3 direction)
    {
        GameObject bullet = Instantiate(m_bullet, RayCastRotation.instance.ShootGO.transform.position, Quaternion.identity);
        bullet.GetComponent<BulletBehaviours>().MoveDirection = direction;
        bullet.GetComponent<BulletBehaviours>().Speed = 2f;

        bullet.GetComponent<BulletBehaviours>().LaunchBullet();
    }

    void PlayerMovement(float moveHorizontal, float moveVertical)
    {

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0).normalized;

        Controller.Move(movement.normalized * m_DashPower * Time.deltaTime);
    }


    public void NewOrientation(GameObject newOrientation)
    {
        if (orientation != null)
        {
            orientation.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

        }
        orientation = newOrientation;
        orientation.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    #region Dash
    void Dash(float moveHorizontal, float moveVertical)
    {
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0).normalized;
        DisableColliderOnDash();

        m_DashDuration.ResetPlay();
        transform.DOMove(movement, m_DashTimer);
    }

    void DashEnable()
    {
        m_CanDash = true;
    }

    void DashDisable()
    {
        m_CanDash = false;
    }


    #endregion

    #region Deflect
    void DeflectEnable()
    {
        m_CanDeflect = true;
    }

    void DeflectDisable()
    {
        m_CanDeflect = false;
    }
    #endregion

    #region Collision
    private void DisableColliderOnDash()
    {
        m_Collider.enabled = false;
    }

    private void EnableCollider()
    {
        m_Collider.enabled = true;
    }
    #endregion


    #region GETTER && SETTER

    public CharacterController Controller { get => m_Controller; set => m_Controller = value; }
    public LayerMask BulletLayer { get => bulletLayer; set => bulletLayer = value; }
    public float MinDistanceToPerfect { get => minDistanceToPerfect; set => minDistanceToPerfect = value; }
    public float MaxDistanceToPerfect { get => maxDistanceToPerfect; set => maxDistanceToPerfect = value; }

    #endregion



}
