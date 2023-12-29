using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff {

    private float tickTime;
    private float timeSinceTick;
    private PoisonSplash splashPrebfab;

    private int splashDamage;


    public PoisonDebuff(int splashDamage, float tickTime, PoisonSplash splashPrebfab, float duration, Monster target) : base(target, duration)
    {
        this.splashDamage = splashDamage;
        this.tickTime = tickTime;
        this.splashPrebfab = splashPrebfab;
        target.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public override void Update()
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;

            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                Splash();
            }
        }
        base.Update();
    }

    private void Splash()
    {
        PoisonSplash tmp = GameObject.Instantiate(splashPrebfab, target.transform.position, Quaternion.identity);

        tmp.Damage = splashDamage;

        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
    }

    public override void Remove()
    {
        target.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        base.Remove();
    }
}
