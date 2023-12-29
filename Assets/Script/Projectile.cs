using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private Monster target;
    private Tower parent;
    private Animator myAnimator;

    private Element elementType;

    public void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveToTarget();
    }

    /*setting the parent and target of projectile*/
    public void Initialize(Tower parent)
    {
        this.target = parent.target;
        this.parent = parent;

        this.elementType = parent.ElementType;
    }

    /*Move the projectile towards target */
    private void MoveToTarget()
    {
       if(target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.projectileSpeed);
            Vector2 direction = target.transform.position - transform.position;

            /*Atan2 == arctan */
            /*Atan2 (y : float, x : float)*/
            /*以弧度为单位计算并返回 y/x 的反正切值*/
            /*Atan2能返回第二三象限的值*/
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            /*绕Axis轴，旋转Angle角度。例如：绕Y轴旋转30度。*/
            /*Quaternion.AngleAxis(30,Vector3.up);//Vector3.up代表的是Y轴正方向。*/
            /*Vector3.forward(它代表Vector3(0,0,1))*/
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
       else if (!target.IsActive)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject); /*Disactive the gameObject*/
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {
            if(target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.damage, elementType);

                 myAnimator.SetTrigger("Impact");
                 // GameManager.Instance.Pool.ReleaseObject(gameObject);

                 ApplyDebuff();
            }
        }
    }

    private void ApplyDebuff()
    {
        if(target.ElementType != elementType)
        {
            float roll = Random.Range(0, 100);

            if(roll <= parent.proc)
            {
                target.AddDebuff(parent.GetDebuff());
            }
        }
    }
}
