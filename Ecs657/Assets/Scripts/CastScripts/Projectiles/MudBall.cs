using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MudBall : GenericProjectile
{
    protected override IEnumerator projEffect()
    {
        enemyObj.GetComponent<NavMeshAgent>().speed /= 2;
        enemy.wet = true;
        yield return new WaitForSeconds(duration);
        enemy.wet = false;
        enemyObj.GetComponent<NavMeshAgent>().speed *= 2;
        effectEnd = true;
    }
}
