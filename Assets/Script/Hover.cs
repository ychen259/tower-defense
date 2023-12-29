using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : singleTon<Hover> {

    public SpriteRenderer spriteRenderer; /*塔的图像*/

    private SpriteRenderer rangeSpriteRenderer; /*塔攻击方位的图像*/

    public bool IsVisible;

    public void Start()
    {
        rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public  void Update()
    {
        FollowMouse();

        double scaleX;
        double scaleY;

        scaleX = 5.6 / transform.localScale.x;
        scaleY = 5.6 / transform.localScale.y;
        transform.GetChild(0).transform.localScale = new Vector3((float)scaleX, (float)scaleY, 1); /*translate 塔攻击范围的scale*/
    }

    /*hove 跟随鼠标的位置*/
    public void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    /*让hover是tower的图像*/
    public void Activate(Sprite sprite, bool isTower)
    {
        this.spriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
        
        this.transform.localScale = GameManager.Instance.hoverScale;

        /*if it is block, dont show the attack range*/
        if(isTower)
          rangeSpriteRenderer.enabled = true; /*enable 攻击范围*/


        IsVisible = true;
    }

    /*取消hover的图像*/
    public void Deativate()
    {
        this.spriteRenderer.enabled = false;
        GameManager.Instance.ClickedBtn = null; /*设置clicked button为null*/
        rangeSpriteRenderer.enabled = false; /*disable hover的攻击范围*/

        IsVisible = false;
    }
}
