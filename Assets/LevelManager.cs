using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    public GameObject SpawnerCentral;
    public float spawnerRadius = 5f;

    public GameObject batPrefabs;

    public int wave;

    public int unitKill;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : LevelManager");
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isWaveFinish()
    {
        unitKill++;
        return unitKill >= GameManager.instance.enemies;
    }

    public void UnitIsKill()
    {
        if (isWaveFinish())
        {
            wave++;
            NewWave(wave);
        }
    }

    public void NewWave(int wave = 1)
    {
        GameManager.instance.enemies = wave;
        ScoreManager.instance.GainByNoDamage();

        for (int i = 0; i < wave; i++)
        {
            Vector3 pos = Random.insideUnitCircle * spawnerRadius;
            pos.z = 0;

            Instantiate(batPrefabs, pos, Quaternion.identity);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(SpawnerCentral.transform.position, spawnerRadius);
    }
}
