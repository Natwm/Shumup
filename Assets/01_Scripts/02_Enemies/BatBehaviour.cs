using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BatBehaviour : BaseEnemyBehaviours
{

    private List<Vector3> pos;
    // Start is called before the first frame update
    void Start()
    {
        //pos = gameObject.transform.position + new Vector3(0, 5, 0));

        
        moveBat(new Vector3(0, Random.Range(0, -5), 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
