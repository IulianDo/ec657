using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LavaBall : GenericProjectile
{
    protected override IEnumerator projEffect()
    {
        StartCoroutine(sideEffect());
        while (enemyObj != null)
        {
            enemy.TakeDamage(Mathf.RoundToInt(1 * dmgMul));
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator sideEffect()
    {
        enemyObj.GetComponent<NavMeshAgent>().speed /= 2;
        yield return new WaitForSeconds(duration);
        enemyObj.GetComponent<NavMeshAgent>().speed *= 2;
        effectEnd = true;
    }
}
