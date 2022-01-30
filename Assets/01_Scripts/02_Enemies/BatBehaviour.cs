using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BatBehaviour : BaseEnemyBehaviours
{
    public float coolDownLine = 2.0f;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 1;
    public Material lineMat;
    [SerializeField] public LayerMask layer;
    [SerializeField] public int damageGiven;
    Coroutine co;
    Timer timer;
    public GameObject prefab;
    public Vector3 dir;

    // Start is called before the first frame update
    //usage example
    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMat;
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = lengthOfLineRenderer;
        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        timer = new Timer(1, test);
        this.moveBat(new Vector3(0, Random.Range(0, -5), 0), true);
    }
    void test()
    {
        if(gameObject.transform.gameObject != null)
        {
            this.moveBat(new Vector3(0, Random.Range(0, -5), 0), true);
        }

    }
    IEnumerator FireMyLaser()
    {
        Vector3 playerPosition = CharacterBehaviours.instance.gameObject.transform.position;
        yield return new WaitForSeconds(coolDownLine);
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        RaycastHit hit;
        lineRenderer.positionCount = 0;
        // test sans raycast, car shoot et non pas laser
       
        int random = GetRandomValue(new RandomSelection(0, 5, .5f), new RandomSelection(6, 8, .3f), new RandomSelection(9, 10, .2f));
        if (random < 5)
        {
            //Le vrai truc de lancer (proba de shoot)
            GameObject obj = Instantiate(prefab, gameObject.transform.position + new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            obj.GetComponent<BulletBehaviours>().LaunchBullet(dir, 10f);
        }

        //Destroy(obj, 15f);
        if (Physics.Raycast(gameObject.transform.position, playerPosition - gameObject.transform.position, out hit, Mathf.Infinity, layer))
        {
            Debug.Log("jfidosqfoisd");
            random = GetRandomValue(new RandomSelection(0, 5, .5f), new RandomSelection(6, 8, .3f), new RandomSelection(9, 10, .2f));


            if (random < 5)
            {
                //Le vrai truc de lancer (proba de shoot)
                GameObject obj = Instantiate(prefab, gameObject.transform.position + new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
                obj.GetComponent<BulletBehaviours>().LaunchBullet(dir, 10f);
            }
            if (hit.transform.gameObject.CompareTag("Player") && random < 5)
            {
                // LASER IF, replace with bulletshot
                //Debug.Log(random);
                //Debug.DrawRay(gameObject.transform.position, playerPosition - gameObject.transform.position, Color.red, 2);
                //CharacterBehaviours.instance.TakeDamage(damageGiven);

                //lineRenderer.positionCount = 2;
                //lineRenderer.SetPosition(0, gameObject.transform.position);
                //lineRenderer.SetPosition(1, playerPosition);
                //Debug.Log("touched " + hit.transform.gameObject);
            }

        }


        yield return new WaitForSeconds(coolDownLine);
        this.localMove();


    }
    protected virtual void moveBat(Vector3 pos, bool isBat = false)
    {
        transform.DOMove(pos, 2)
        .SetEase(Ease.OutQuint)
        .SetLoops(-1, LoopType.Yoyo)
        .OnStepComplete(() =>
        {
            co = StartCoroutine(FireMyLaser());

        });
    }
    protected virtual void localMove()
    {
        Vector3 position = Random.insideUnitSphere * 5;
        Ray ray = new Ray(gameObject.transform.position, Vector3.forward);
        RaycastHit hit;
/*        if (Physics.SphereCast(ray, 5, out hit))
        {
            if (hit.transform.gameObject.CompareTag("bat")) {
                position = Random.insideUnitSphere * 5;
            }
        }*/
        position = new Vector3(position.x, position.y, 0);
        transform.DOMove(position, 2)
        .SetEase(Ease.OutQuint)
        .OnStepComplete(() =>
        {
            StopCoroutine(co);
            
            timer.ResetPlay();
           
        });
    }
    // Update is called once per frame
    void Update()
    {
    }

}
