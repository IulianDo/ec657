using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waterball : GenericProjectile
{
    private float speed;

    // Slows down enemies affected by projectile
    protected override IEnumerator projEffect()
    {
        //enemyObj.GetComponent<NavMeshAgent>().speed /= 2;
        yield return new WaitForSeconds(duration);
        effectEnd = true;
        //enemyObj.GetComponent<NavMeshAgent>().speed *= 2;
        //duration = 0;
    }
    
}
