using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    [SerializeField] public TMP_Text score;

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

    public void ChangeScore(float points)
    {

        score.text = points + "";
    }
    public void StartWave()
    {

        playerStatLife = CharacterBehaviours.instance.Health;
        ChangeScore(globalScore);
    }


    public void GainByKill()
    {
        globalScore += killScore;
        ChangeScore(globalScore);
    }

    public void GainByPerfectShoot()
    {
        globalScore += perfectShootScore;
        ChangeScore(globalScore);
    }

    public void GainByNoDamage()
    {
        if (playerStatLife == CharacterBehaviours.instance.Health)
            globalScore += withoutDamageScore;
            ChangeScore(globalScore);
    }

    public void LooseByDamage()
    {
        globalScore += damageScore;
        ChangeScore(globalScore);
    }

    public float GlobalScore { get => globalScore; set => globalScore = value; }


}
