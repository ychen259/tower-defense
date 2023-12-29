using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff {

    protected Monster target;

    private float duration; /*buff的生存时间*/

    private float elapsed;/*多少时间过去*/

    public Debuff(Monster target, float duration)
    {
        this.target = target;
        this.duration = duration;
    }

    public virtual void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= duration)
        {
            elapsed = 0; /*MY code*/
            Remove();
        }
    }

    public virtual void Remove()
    {
        if(target != null)
        {
            target.RemoveDebuff(this);
        }
    }
}
