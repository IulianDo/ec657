using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : GenericProjectile
{



    private void Start()
    {
        
    }

    protected override IEnumerator projEffect()
    {
        while (duration > 0)
        {
            if(enemyObj != null)
            {
                enemy.TakeDamage(1);
                duration--;
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }
}
