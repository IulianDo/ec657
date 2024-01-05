using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlizzardController : GenericParticles
{
    protected override void ParticleEffect(Enemy enemy)
    {
        enemy.ApplyEffect("Slow", damage, time, damageInterval);
        enemy.ApplyEffect("Wet", damage, time, damageInterval);
    }

}
