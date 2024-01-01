using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBall : GenericProjectile
{
    protected override IEnumerator projEffect()
    {
        yield return new WaitForEndOfFrame();
        effectEnd = true;
    }
}
