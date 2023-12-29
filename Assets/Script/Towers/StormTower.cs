using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTower : Tower{

    public void Start()
    {
        ElementType = Element.STORM;

        Upgrades = new TowerUpgrade[]
        {
            /*(int price, int damage, float debuffDuration, float procChance)*/
            new TowerUpgrade(2, 1, 1, 2), /*first upgrade*/
            new TowerUpgrade(2, 1, 1, 2), /*second upgrade*/
        };
    }

    public override Debuff GetDebuff()
    {
        return new StormDebuff(target, debuffDuration);
    }

    public override string GetStats()
    {
        return string.Format("<color=#add8e6ff><size=20><b>{0}</b></size></color>{1}", "Storm Tower", base.GetStats());
    }
}
