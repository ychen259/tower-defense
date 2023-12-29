using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade  {

    public int Price;
    public int Damage;
    public float DebuffDuration;
    public float ProcChance;
    public float SlowingFactor;
    public float TickTime;
    public int SpecialDamage;

    /*4*/
    public TowerUpgrade(int price, int damage, float debuffDuration, float procChance)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffDuration;
        this.ProcChance = procChance;
        this.Price = price;
    }

    /*5*/
    public TowerUpgrade(int price, int damage, float debuffDuration, float procChance, float slowingFactor)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffDuration;
        this.ProcChance = procChance;
        this.SlowingFactor = slowingFactor;
        this.Price = price;
    }

    /*6*/
    public TowerUpgrade(int price, int damage, float debuffDuration, float procChance, float tickTime, int specialDamage)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffDuration;
        this.ProcChance = procChance;
        this.TickTime = tickTime;
        this.SpecialDamage = specialDamage;
        this.Price = price;
    }
}
