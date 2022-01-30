using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BaseEnemyBehaviours : TokenBehaviours
{

    //helper structure
    public struct RandomSelection
    {
        private int minValue;
        private int maxValue;
        public float probability;

        public RandomSelection(int minValue, int maxValue, float probability)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.probability = probability;
        }

        public int GetValue() { return Random.Range(minValue, maxValue + 1); }
    }

    protected int GetRandomValue(params RandomSelection[] selections)
    {
        float rand = Random.value;
        float currentProb = 0;
        foreach (var selection in selections)
        {
            currentProb += selection.probability;
            if (rand <= currentProb)
                return selection.GetValue();
        }

        //will happen if the input's probabilities sums to less than 1
        //throw error here if that's appropriate
        return -1;
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

    public override void TakeDamage(int _AmountOfDamage)
    {
        if (m_CanBeHit)
        {
            Health -= _AmountOfDamage;
            hitEffect.start();

            ScoreManager.instance.GainByKill();

            if (IsDead())
                Dead();
        }

    }
}
