using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BatBehaviour : BaseEnemyBehaviours
{
    private static List<Vector3> playerPosition;
    public float coolDownLine = 2.0f;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 1;
    public Material lineMat;
    [SerializeField] public LayerMask layer;
    [SerializeField] public int damageGiven;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = new List<Vector3>();
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
        this.moveBat(new Vector3(0, Random.Range(0, -5), 0), true);
    }
    IEnumerator FireMyLaser()
    {
        Vector3 playerPosition = CharacterBehaviours.instance.gameObject.transform.position;
        yield return new WaitForSeconds(coolDownLine);
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        RaycastHit hit;
        
        if (Physics.Raycast(gameObject.transform.position, playerPosition - gameObject.transform.position, out hit, Mathf.Infinity, layer))
        {
            Debug.Log(hit.transform.gameObject);
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                CharacterBehaviours.instance.TakeDamage(damageGiven);
                Debug.Log("touched " + hit.transform.gameObject);
            }
            
        }
        Debug.DrawRay(gameObject.transform.position, playerPosition - gameObject.transform.position, Color.red, 2);
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, playerPosition);
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
            Coroutine co = StartCoroutine(FireMyLaser());

        });
    }
    protected virtual void localMove()
    {
        Vector3 position = Random.insideUnitSphere * 5;
        position = new Vector3(position.x, position.y, 0);
        transform.DOMove(position, 2)
        .SetEase(Ease.OutQuint)
        .OnStepComplete(() =>
        {
            StopCoroutine("FireMyLaser");
            this.moveBat(new Vector3(0, Random.Range(0, -5), 0));
        });
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0, 100f, 0.0f)), Color.red);
    }

}
