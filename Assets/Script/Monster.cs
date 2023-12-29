using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {

    public float speed; // moving speed
    public float MaxSpeed;

    public Stack<node> path; // path from start to destination

    /*Because debuffs List need to be run every update function, so we cannot add or remove duration the update function*/
    /*Therefore, we need debuffsToRemove List and newDebuffs to help us to finish thing that we want to do*/
    private List<Debuff> debuffs = new List<Debuff>();
    private List<Debuff> debuffsToRemove = new List<Debuff>();
    private List<Debuff> newDebuffs = new List<Debuff>();

    public Point Gridposition; // get the gridposition of destination
    public Vector3 destination; // world positio of the destination

    private SpriteRenderer spriteRenderer;

    public bool IsActive; /*isActive == false (the monster is chaning its size)*/
                            /*isActive == true （the monster is moving）*/

    private Animator myAnimator;

    public Image healthBar;

    private float health; // current health of monster
    private int maxHealth; // max health of monster

    [SerializeField]
    private Element elementType;

    private int invulberability = 2; /*攻击和怪物同一类型的减伤害*/

    public bool Alive
    {
        get { return health > 0; }
    }

    public Element ElementType
    {
        get
        {
            return elementType;
        }

        set
        {
            elementType = value;
        }
    }

    public List<Debuff> Debuffs
    {
        get
        {
            return debuffs;
        }

        set
        {
            debuffs = value;
        }
    }

    private void Awake()
    {
        healthBar = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        MaxSpeed = speed;
    }

    public void Update()
    {
        float percentage = speed / MaxSpeed;
        MaxSpeed = LevelManager.Instance.Tiles[Gridposition].monsterSpeed; /*change monster moving speed to the limit of tile speed*/
        speed = percentage * MaxSpeed;

        HandleDebuffs();

        Move();

        /*update the health bar*/
        healthBar.fillAmount = (float)this.health / (float)maxHealth; /*percentage of health to 100%*/
    }

    /*set the start position of monster to the blue portal*/
    /*and grow up the sacel of monster depend on timing*/
    public void Spwan(int health)
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        speed = MaxSpeed;

        Debuffs.Clear();

        debuffsToRemove.Clear();
        newDebuffs.Clear();

        maxHealth = health; //maxHealth
        this.health = health; //current health
        healthBar.fillAmount = (float)this.health/ (float)maxHealth; /*percentage of health to 100%*/


        myAnimator = GetComponent<Animator>();

        transform.position = LevelManager.Instance.BluePortal.transform.position;   /*set the start position of monster to the blue portal*/

        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));   /*and grow up the scale of monster depend on timing*/

        SetPath(LevelManager.Instance.Path); //execute without wait for StartCoroutine
    }

    /*Change the scale of monster in enter and exit*/
    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {

        float progress = 0;
        
        while(progress <= 1)
        {
            /*两个向量之间的线性插值。按照数字t在from到to之间插值。*/
            /*progress 是夹在0到1之间。当t=0时，返回from。当t=1时，返回to。当t=0.5时放回from和to之间的平均数。*/
            transform.localScale = Vector3.Lerp(from, to, progress);

            progress += Time.deltaTime;

            yield return null;
        }

        transform.localScale = to;

        IsActive = true;

        if (remove)
        {
            Release(); /**/
        }
    }

    /*move the monster depend on the path*/
    public void Move()
    {
        /*while the monster is not active (changing its scale), dont move*/
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            /*when the monster's position == to next position, then find out new desition*/
            if (transform.position == destination)
            {
                if (path != null && path.Count > 0) 
                {
                    Animate(Gridposition, path.Peek().GridPosition); /*play different animation depend on the situation*/

                    Gridposition = path.Peek().GridPosition; /*get the gridposition of next position*/
                    destination = path.Pop().worldPosition; /*the the world position of next position*/
                }
            }
        }
    }

    /*find the next destination using path*/
    private void SetPath(Stack<node> newPath)
    {
        if(newPath != null) 
        {
            this.path = newPath;
            Animate(Gridposition, path.Peek().GridPosition);
            Gridposition = path.Peek().GridPosition;
            destination = path.Pop().worldPosition;
        }
    }

    /*play differnt animations depend on the situation*/
    public void Animate(Point currentPos, Point newPos)
    {
        if(currentPos.y > newPos.y)
        {
            //moving up
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", 1);
        }
        else if(currentPos.y < newPos.y)
        {
            //moving down
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", -1);
        }
        if(currentPos.y == newPos.y)
        {
            if(currentPos.x > newPos.x)
            {
                //move to left
                myAnimator.SetInteger("Horizontal", -1);
                myAnimator.SetInteger("Vertical", 0);
            }
            else if(currentPos.x < newPos.x)
            {
                //move to right
                myAnimator.SetInteger("Horizontal", 1);
                myAnimator.SetInteger("Vertical", 0);
            }
        }
    }

    /*If the monster is hit with the redPortal, then change is scale and destory it, and decrease the lives*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RedPortal")
        {
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));

            GameManager.Instance.lives--;
        }

        if(other.tag == "Tile")
        {
            spriteRenderer.sortingOrder = other.GetComponent<TileScript>().GridPosition.y;
        }
    }

    /*reuse the monster from the pool*/
    private void Release()
    {
        Debuffs.Clear();
        debuffsToRemove.Clear();
        newDebuffs.Clear();

        IsActive = false;
        Gridposition = LevelManager.Instance.blueSpawn;
        GameManager.Instance.Pool.ReleaseObject(gameObject); /*DisActive the gameObject*/

        GameManager.Instance.RemoveMonster(this);
    }

    public void TakeDamage(float damage, Element dmgSource)
    {
        if (IsActive)
        {
            if(dmgSource == ElementType)
            {
                damage = damage / invulberability;
                //invulberability++;
            }
            health -= damage;
        }   

        if(health <= 0)
        {
            SoundManager.Instance.PlaySFX("die");

            GameManager.Instance.Currency += 2;
            IsActive = false;

            GetComponent<SpriteRenderer>().sortingOrder--; //put die monster one the back of other monster
            Release();
        }
    }

    public void AddDebuff(Debuff debuff)
    {
        if(!Debuffs.Exists(x=> x.GetType() == debuff.GetType()))
        {
            newDebuffs.Add(debuff);
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        debuffsToRemove.Add(debuff);
    }

    private void HandleDebuffs()
    {
        if(newDebuffs.Count > 0)
        {
            Debuffs.AddRange(newDebuffs); /*add everything from newDebuffs list to debuffs*/

            newDebuffs.Clear(); /*remove everything from newDebuffs List*/
        }

        foreach(Debuff debuff in debuffsToRemove)
        {
            Debuffs.Remove(debuff);
        }

        debuffsToRemove.Clear();

        foreach(Debuff debuff in Debuffs)
        {
            debuff.Update();
        }
    }

}
