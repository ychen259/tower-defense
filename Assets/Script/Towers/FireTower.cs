using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower {

    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamage;

    public float TickTime
    {
        get
        {
            return tickTime;
        }

        set
        {
            tickTime = value;
        }
    }

    public float TickDamage
    {
        get
        {
            return tickDamage;
        }

        set
        {
            tickDamage = value;
        }
    }

    public void Start()
    {
        ElementType = Element.FIRE;
        Upgrades = new TowerUpgrade[]
        {
            /*(int price, int damage, float debuffDuration, float procChance, float tickTime, int specialDamage)*/
            new TowerUpgrade(2, 2, 0.5f, 5, -0.1f, 1), /*first upgrade*/
            new TowerUpgrade(5, 3, 0.5f, 5, -0.1f, 1), /*second upgrade*/
        };
    }

    public override Debuff GetDebuff()
    {
        return new FireDebuff(TickDamage, tickTime, debuffDuration, target); /*target is monster from Tower Script*/
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nTick damage: {3} <color=#00ff00ff> +{5}</color>", "<size=20><b>Fire Tower></b></size>", base.GetStats(), TickTime, TickDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        return string.Format("<color=ffa500ff>{0}</color>{1} \nTick time: {2}\nTick damage: {3}", "<size=20><b>Fire tower</b></size>", base.GetStats(), TickTime, TickDamage);
    }

    public override void Upgrade()
    {
        this.tickTime += NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }
}
