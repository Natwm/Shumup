using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BaseEnemyBehaviours : TokenBehaviours
{

    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected void localMove()
    {
        Vector3 position = Random.insideUnitSphere * 5;
        transform.DOMove(position, 2)
        .SetEase(Ease.OutQuint)
        .OnStepComplete(() => {

            moveBat(new Vector3(0, Random.Range(0, -5), 0)); 
        });
    }
    protected void moveBat(Vector3 pos, bool isBat = false)
    {
        transform.DOMove(pos, 2)
        .SetEase(Ease.OutQuint)
        .SetLoops(-1, LoopType.Yoyo)
        .OnStepComplete(() => { 
            localMove(); 
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
