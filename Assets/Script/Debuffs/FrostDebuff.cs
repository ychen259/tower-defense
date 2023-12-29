using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostDebuff : Debuff {

    private float slowingFactor;

    private bool applied; /*slow the monster once rather than double slow them */

    public FrostDebuff(float slowingFactor, float duration, Monster target) : base(target, duration)
    {
        this.slowingFactor = slowingFactor;
    }

    public override void Update()
    {
        if (target != null)
        {
            if (!applied)
            {
                applied = true;
                target.speed -= (target.MaxSpeed * slowingFactor) / 100;
                target.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
        base.Update();
    }

    public override void Remove()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            applied = false;
            target.speed = target.MaxSpeed;

            base.Remove();
        }
    }
}
