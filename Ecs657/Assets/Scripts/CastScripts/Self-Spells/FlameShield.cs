using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameShield : ShieldController
{
    protected override void projEffect()
    {
        enemy.ApplyEffect("Burn", factor, duration, interval);
    }
}
