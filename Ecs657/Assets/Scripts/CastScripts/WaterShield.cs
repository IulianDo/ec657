using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShield : ShieldController
{
    protected override void projEffect()
    {
        enemy.ApplyEffect("Wet", factor, duration, 0);
    }
}
