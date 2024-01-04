using UnityEngine;
using System.Collections;

public class FlamethrowerController : GenericParticles
{
    protected override void ParticleEffect(Enemy enemy)
    {
        enemy.ApplyEffect("Burn", damage, time, damageInterval);
    }
}