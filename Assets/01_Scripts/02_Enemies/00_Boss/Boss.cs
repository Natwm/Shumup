using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChangeStatus{

    public Boss.BossStatus currentStatus;
    public List<Boss.BossStatus> possibleStatus = new List<Boss.BossStatus>();

    public ChangeStatus(Boss.BossStatus currentStatus, Boss.BossStatus first, Boss.BossStatus second, Boss.BossStatus third)
    {
        this.currentStatus = currentStatus;
        possibleStatus.Add(first);
        possibleStatus.Add(second);
        possibleStatus.Add(third);
    }

    public ChangeStatus(Boss.BossStatus currentStatus, Boss.BossStatus first, Boss.BossStatus second)
    {
        this.currentStatus = currentStatus;
        possibleStatus.Add(first);
        possibleStatus.Add(second);
    }

    public ChangeStatus(Boss.BossStatus currentStatus, Boss.BossStatus first)
    {
        this.currentStatus = currentStatus;
        possibleStatus.Add(first);
    }

}

public class Boss : BaseEnemyBehaviours
{

    public enum BossStatus
    {
        IDLE,
        ATTACK,
        DEFENSE,
        ZONING
    }

    [SerializeField] private BossStatus currentStatus = BossStatus.IDLE;
    [SerializeField] private List<ChangeStatus> possibleChangeStatus = new List<ChangeStatus>();


    public Vector3 startingPosition;

    bool isMoving;

    

    [SerializeField] private GameObject bulletSpawner;
    [SerializeField] private GameObject bulletPrefabs;


    [Space]
    [Header("Param Modifiable")]
    [SerializeField] private float m_CurrentSpeed;
    [Header("Movement")]
    [SerializeField] private float m_IdleSpeed;
    [SerializeField] private float m_AttackSpeed;
    [SerializeField] private float m_DefenseSpeed;
    [SerializeField] private float m_ZoningSpeed;

    [Space]
    [Header("Rush Attaque Param")]
    [SerializeField] private float m_TimeBeforeRushOnPlayer;

    [Space]
    [Header("Boulet Attaque Param")]
    [SerializeField] private GameObject bouletGO;
    [SerializeField] private float bouletSpeed;
    [SerializeField] private float bouletPrecision;
    [SerializeField] private float m_TimeBeforeLaunchBoulet;
    [SerializeField] private float m_TimeBeforeRushToBoulet;

    [Space]
    [Header("Defense Rush param")]
    [SerializeField] private Transform baseTP;
    [SerializeField] private int m_AmountOfTPMax;
    [SerializeField] private List<Transform> m_TpSpot;
    [SerializeField] private float m_TimeDefenseRush;
    [SerializeField] private float m_TimeBeforTp;

    FMOD.Studio.EventInstance changePhaseEffect;
    [SerializeField] private FMODUnity.EventReference changePhaseSound;

    // Start is called before the first frame update
    void Start()
    {
        ChangeStatus idle = new ChangeStatus(BossStatus.IDLE, BossStatus.ATTACK, BossStatus.DEFENSE, BossStatus.ZONING);
        ChangeStatus attack = new ChangeStatus(BossStatus.ATTACK, BossStatus.IDLE, BossStatus.DEFENSE, BossStatus.ZONING);
        ChangeStatus defense = new ChangeStatus(BossStatus.DEFENSE, BossStatus.IDLE, BossStatus.ATTACK, BossStatus.ZONING);
        ChangeStatus zoning = new ChangeStatus(BossStatus.ZONING, BossStatus.IDLE, BossStatus.ATTACK, BossStatus.DEFENSE);

        possibleChangeStatus.Add(idle);
        possibleChangeStatus.Add(attack);
        possibleChangeStatus.Add(defense);
        possibleChangeStatus.Add(zoning);

        startingPosition = transform.position;

        m_FireTimer = new Timer(m_FireRate, ShootEnable);
        SetUpFmod();
    }

    public virtual void SetUpFmod()
    {
        deathEffect = FMODUnity.RuntimeManager.CreateInstance(deathSound);
        hitEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);
        basicAttaqueEffect = FMODUnity.RuntimeManager.CreateInstance(basicAttaqueSound);
        spawnEffect = FMODUnity.RuntimeManager.CreateInstance(spawnSound);
        hitEffect = FMODUnity.RuntimeManager.CreateInstance(hitSound);

        changePhaseEffect = FMODUnity.RuntimeManager.CreateInstance(changePhaseSound);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            DefenseRushAttaque();

        if(FindObjectsOfType<BulletBehaviours>().Length < 1 && m_CanShoot)
        {
            m_CanShoot = false;
            m_FireTimer.ResetPlay();

            GameObject bullet = Instantiate(bulletPrefabs, bulletSpawner.transform.position, Quaternion.identity);
            Vector3 pos = new Vector3(CharacterBehaviours.instance.transform.position.x - bulletSpawner.transform.position.x, CharacterBehaviours.instance.transform.position.y - bullet.transform.position.y);
            bullet.GetComponent<BulletBehaviours>().LaunchBullet(pos.normalized,5f);
            Destroy(bullet, 10f);
        }
    }

    #region Change Status
    private void ChangeStatus(BossStatus status)
    {
        switch (status)
        {
            case BossStatus.IDLE:
                ChangeToIdleStatus();
                break;
            case BossStatus.ATTACK:
                ChangeToAttackStatus();
                break;
            case BossStatus.DEFENSE:
                ChangeToDefenseStatus();
                break;
            case BossStatus.ZONING:
                ChangeToZoningStatus();
                break;
            default:
                break;
        }
    }

    private void ChangeToIdleStatus()
    {
        ChangeSpeed(m_IdleSpeed);
    }

    private void ChangeToAttackStatus()
    {
        ChangeSpeed(m_AttackSpeed);
    }

    private void ChangeToDefenseStatus()
    {
        ChangeSpeed(m_DefenseSpeed);
    }

    private void ChangeToZoningStatus()
    {
        ChangeSpeed(m_ZoningSpeed);
    }
    #endregion



    private void ChangeSpeed(float newSpeed)
    {
        m_CurrentSpeed = newSpeed;
    }


    private IEnumerator RushToThePlayer()
    {
        Vector3 playerPosition = new Vector3((CharacterBehaviours.instance.transform.position.x - transform.position.x), (CharacterBehaviours.instance.transform.position.y - transform.position.y), 0); ;

        yield return new WaitForSeconds(m_TimeBeforeRushOnPlayer);

        GetComponent<Rigidbody>().velocity = playerPosition.normalized * m_CurrentSpeed;

    }

    private IEnumerator LaunchBoulet()
    {
        Vector3 precision = Random.insideUnitCircle * bouletPrecision;
        print(precision);
        Vector3 playerPosition = new Vector3((CharacterBehaviours.instance.transform.position.x - bouletGO.transform.position.x), (CharacterBehaviours.instance.transform.position.y - bouletGO.transform.position.y),0)+ precision;
        print(playerPosition.normalized);
        yield return new WaitForSeconds(m_TimeBeforeLaunchBoulet);

        bouletGO.GetComponent<Rigidbody>().velocity = playerPosition.normalized * bouletSpeed;

        yield return new WaitForSeconds(m_TimeBeforeRushToBoulet);
        Vector3 bouletPosition = new Vector3((bouletGO.transform.position.x - transform.position.x), (bouletGO.transform.position.y - transform.position.y), 0);


        GetComponent<Rigidbody>().velocity = bouletPosition * m_CurrentSpeed;
    }


    private void DefenseRushAttaque()
    {
        m_CanBeHit = false;
        int amountOfTP = Random.Range(0, m_AmountOfTPMax + 1);
        print(amountOfTP);

        StartCoroutine(DefenseRush(amountOfTP));

    }

    private IEnumerator DefenseRush(int amountOfRush)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 rushDirection = new Vector3((CharacterBehaviours.instance.transform.position.x - transform.position.x), (CharacterBehaviours.instance.transform.position.y - transform.position.y), 0);
        yield return new WaitForSeconds(m_TimeDefenseRush);

        GetComponent<Rigidbody>().velocity = rushDirection * m_CurrentSpeed;

        yield return new WaitForSeconds(m_TimeBeforTp);

        if (amountOfRush > 0)
        {
            int index = Random.Range(0, m_TpSpot.Count + 1);
            

            transform.position = m_TpSpot[index].position;

            StartCoroutine(DefenseRush(amountOfRush - 1));
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = baseTP.transform.position;

            transform.DOMove(startingPosition, 2f);
            m_CanBeHit = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterBehaviours.instance.TakeDamage(1);
        }
    }


}
