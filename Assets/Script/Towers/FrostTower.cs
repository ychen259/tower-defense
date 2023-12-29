using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTower : Tower {

    public float slowingFactor;

    public void Start()
    {
        ElementType = Element.FROST;

        Upgrades = new TowerUpgrade[]
        {
            /*(int price, int damage, float debuffDuration, float procChance, float tickTime, int specialDamage)*/
            new TowerUpgrade(2, 1, 1, 2, 10), /*first upgrade*/
            new TowerUpgrade(2, 1, 1, 2, 10), /*first upgrade*/
        };
    }

    public override Debuff GetDebuff()
    {
        return new FrostDebuff(slowingFactor, debuffDuration, target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#00ffffff>{0}</color>{1} \nSlowing Factor: {2}% <color=#00ff00ff>+{3}%</color>", "<size=20><b>Frost Tower></b></size>", base.GetStats(), slowingFactor, NextUpgrade.SlowingFactor);
        }

        return string.Format("<color=#00ffffff>{0}</color>{1} \nSlowing Factor: {2}%", "<size=20><b> Frost Tower ></b></size>", base.GetStats(), slowingFactor);
    }

    public override void Upgrade()
    {
        this.slowingFactor += NextUpgrade.SlowingFactor;
        base.Upgrade();
    }
}
