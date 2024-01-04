using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : GenericProjectile
{
    // Allows fireball to cause damage over time
    protected override IEnumerator projEffect()
    {
        StartCoroutine(sideEffect());
        while (enemyObj != null)
        {
            enemy.TakeDamage(Mathf.RoundToInt(1*dmgMul));
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator sideEffect()
    {
        yield return new WaitForSeconds(duration);
        effectEnd = true;
    }
}
