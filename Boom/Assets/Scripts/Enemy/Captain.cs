using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Enemy,IDamageable
{
    SpriteRenderer sprite;
    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Player_Hit"))
        {
            Healthy-=damage;
            if (Healthy<=0)
            {
                Healthy=0;
                isDead=true;
                anim.SetBool("dead",true);
                
            }
                anim.SetTrigger("hit");
        }
        
    }
    public override void Init()
    {
        base.Init();
        sprite=GetComponent<SpriteRenderer>();
    }
    public override void Update()
    {
        base.Update();
        if (animState==0)
        {
            sprite.flipX=false;
        }
    }
    public override void SkillAttack()
    {
        base.SkillAttack();
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("skillAttack"))
        {
            sprite.flipX=true;
            if (transform.position.x>TargetPoint.transform.position.x)
            {
                transform.position=Vector2.MoveTowards(transform.position,transform.position+Vector3.right,speed*4*Time.deltaTime);
            }else
            {
                transform.position=Vector2.MoveTowards(transform.position,transform.position+Vector3.left,speed*4*Time.deltaTime);
            }
        }
    }
}
