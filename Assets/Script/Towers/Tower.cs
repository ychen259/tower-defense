using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {STORM, FIRE, POISON, FROST, NONE}

/*this scipt is for the range of the tower*/
public abstract class Tower : MonoBehaviour {

    [SerializeField]
    private string projectileType; /*projectiles type*/

    public float projectileSpeed;

    public int damage;

    public float debuffDuration;

    public float proc; /*precentage to apply debuff*/ /*value from 0 to 100*/

    public Element ElementType;

    public int price;

    public int level; /*tower current level*/

    private SpriteRenderer mySpriteRenderer;

    public Monster target;

    private Queue<Monster> monsters = new Queue<Monster>();

    private bool canAttack = true;

    private float attackTimer; /*current time from last attack*/

    [SerializeField]
    private float attackCooldown; // how often we can attack

    public TowerUpgrade[] Upgrades;

    //private Animator myAnimator;

    public TowerUpgrade NextUpgrade
    {
        get
        {
            if(Upgrades.Length > level - 1)
            {
                return Upgrades[level - 1];
            }

            return null;
        }
    }
    // Use this for initialization
    void Awake () {
        //myAnimator = transform.parent.GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        level = 1;
	}

    /*if the range is hide, then show the range*/
    /*if the range is show, then hide the range*/
    public void Select()
    {
        /*if it is flase, then set to true, if it is true, set to false*/
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;

        GameManager.Instance.UpdateUpgradeTip();
    }

    /*makes the tower attack a target*/
    private void Attack()
    {
        /*wait for attackCooldown time to shoot next time*/
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if(attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }

        /*if null target and there has some other monster in the attack 
         range, then attact another target*/
        if(target == null && monsters.Count > 0 && monsters.Peek().IsActive)
        {
            target = monsters.Dequeue();
        }

        /*make sure the target is active == moving*/
        if(target != null && target.IsActive)
        {
            if (canAttack)
            {
                Shoot();

                //myAnimator.SetTrigger("Attack");
                canAttack = false;
            }
        }


        /*if has target but the target is dead*/
        if((target !=null && !target.Alive) || (target != null && !target.IsActive))
        {
            target = null;
        }
        
    }

    public virtual string GetStats()
    {
        if(NextUpgrade != null)
        {
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4} </color>\nProc: {2}% <color=#00ff00ff>+{5}%</color>\nDebuff: {3}sec" +
                "<color=#00ff00ff> +{6}</color>", level, damage, proc, debuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration );
        }

        return string.Format("\nLevel: {0} \nDamage: {1}\nProc: {2}% \nDebuff: {3}sec", level, damage, proc, debuffDuration);
    }

    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;

        SoundManager.Instance.PlaySFX("Shoot");
        projectile.Initialize(this); // set the parent and target of projectile
    }

    /*if the monster enters the attack range*/
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }

    public abstract Debuff GetDebuff();

    public void Update()
    {
        Attack();
    }

    /*if the monster leaves the attack range*/
    public void OnTriggerExit2D(Collider2D other)
    {

       /* if (other.tag == "Monster")
        {
            target = null;
        }*/

        /*当跑出攻击范围的那个不是我们现在攻击的那个，就从monsters LIST删除最开始那个*/
        /*strom和frost有可能使最开始那个落到最后*/
        if(target.gameObject == other.gameObject && other.tag == "Monster")
        {
            target = null;
        }
        else if(other.tag == "Monster" && target.gameObject != other.gameObject)
        {
            monsters.Dequeue();
        }
    }

    public virtual void Upgrade() {
        GameManager.Instance.Currency -= NextUpgrade.Price;
        price += NextUpgrade.Price; /*increase the sell price*/
        this.damage += NextUpgrade.Damage;
        this.proc += NextUpgrade.ProcChance;
        this.debuffDuration += NextUpgrade.DebuffDuration;
        level++;
        GameManager.Instance.UpdateUpgradeTip(); /*hide the text*/
    }

    
}
