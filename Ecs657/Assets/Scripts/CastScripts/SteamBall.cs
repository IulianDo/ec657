using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteamBall : GenericProjectile
{
    protected override IEnumerator projEffect()
    {
        enemy.SetEffect(Enemy.status.Stunned);
        yield return new WaitForSeconds(duration);
        enemy.SetEffect(Enemy.status.Normal);
    }
}
