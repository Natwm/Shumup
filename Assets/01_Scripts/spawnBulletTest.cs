using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBulletTest : MonoBehaviour
{
    // Start is called before the first frame update

    public float rythmeSpawn;

    public GameObject prefab;
    public GameObject SpawnTransform;

    public Vector3 dir;

    void Start()
    {
        InvokeRepeating("Spawn", rythmeSpawn, rythmeSpawn+1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        GameObject obj = Instantiate(prefab, SpawnTransform.transform);

        obj.GetComponent<BulletBehaviours>().LaunchBullet(dir, 10f);

        Destroy(obj, 15f);
    }
}
