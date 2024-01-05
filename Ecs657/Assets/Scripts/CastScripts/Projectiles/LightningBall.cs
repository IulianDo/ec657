using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : GenericProjectile
{
    protected override IEnumerator projEffect()
    {
        if (enemy.wet)
        {
            enemy.TakeDamage(Mathf.RoundToInt(damage*dmgMul));
        }
        enemy.SetEffect(Enemy.status.Stunned);
        yield return new WaitForSeconds(duration);
        enemy.SetEffect(Enemy.status.Normal);
    }
}
