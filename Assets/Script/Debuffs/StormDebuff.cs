using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDebuff : Debuff {

    public StormDebuff(Monster target, float duration) : base(target, duration)
    {
        if (target != null)
        {
            target.speed = 0;
            target.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    public override void Remove()
    {
        if(target != null)
        {
            target.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            target.speed = target.MaxSpeed;
            base.Remove();
        }
    }
}
