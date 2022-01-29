using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    void TakeDamage(int _AmountOfDamage);
    bool IsDead();
    void Dead();

}
