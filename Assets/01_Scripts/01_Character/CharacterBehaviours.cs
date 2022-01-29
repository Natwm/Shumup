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

    [Space]
    [Header("Timer")]
    Timer m_DashDuration;
    Timer m_DashWaitDuration;

    [Space]
    [Header("Flag")]
    [SerializeField] private bool m_CanDash;

    [SerializeField] private GameObject orientation;




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
    }

    // Update is called once per frame
    void Update()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && m_CanShoot)
        {
            print("shoot");
            ShootProjectile();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && m_CanDash)
        {
            DashDisable();
            m_DashWaitDuration.ResetPlay();
            Dash( moveHorizontal,  moveVertical);
        }

        PlayerMovement( moveHorizontal, moveVertical);
    }

    #endregion

    void PlayerMovement(float moveHorizontal, float moveVertical)
    {

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0).normalized;

        Controller.Move(movement.normalized * m_DashPower * Time.deltaTime);
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

    #endregion
    public void NewOrientation(GameObject newOrientation)
    {
        if (orientation != null)
        {
            orientation.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

        }
        orientation = newOrientation;
        orientation.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
