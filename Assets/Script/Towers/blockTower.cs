using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockTower : Tower
{
    public override Debuff GetDebuff()
    {
        return null;
    }

    public void Start()
    {
        ElementType = Element.NONE;
        Upgrades = new TowerUpgrade[]
        {
        };
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            //return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nTick damage: {3} <color=#00ff00ff> +{5}</color>", "<size=20><b>Fire Tower></b></size>", base.GetStats(), TickTime, TickDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        return string.Format("<color=ffa500ff>{0}</color> \nStop enemy to pass it", "<size=20><b>Block</b></size>");
    }

}
