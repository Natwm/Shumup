using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : ScoreManager");
        instance = this;
    }

    [SerializeField] private float globalScore;
    [SerializeField] private float playerStatLife;

    [Space]
    [Header("ScoreParam")]
    [SerializeField] private float killScore = 50;
    [SerializeField] private float perfectShootScore = 25;
    [SerializeField] private float withoutDamageScore = 200;
    [SerializeField] private float damageScore = -10;


    public void StartWave() {

        playerStatLife = CharacterBehaviours.instance.Health;

    }

    public void GainByKill()
    {
        globalScore += killScore;
    }

    public void GainByPerfectShoot()
    {
        globalScore += perfectShootScore;
    }

    public void GainByNoDamage()
    {
        if(playerStatLife == CharacterBehaviours.instance.Health)
            globalScore += withoutDamageScore;
    }

    public void LooseByDamage()
    {
        globalScore += damageScore;
    }

    public float GlobalScore { get => globalScore; set => globalScore = value; }


}
