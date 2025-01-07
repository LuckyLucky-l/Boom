using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : Enemy,IDamageable
{
    public Transform pickupPonint;
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
    public void PickupBomb(){//Animation Event
        if (TargetPoint.CompareTag("Bomb")&&!hasBomb)
        {
            TargetPoint.position=pickupPonint.position;
            TargetPoint.GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Kinematic;
            TargetPoint.SetParent(pickupPonint);
        }
        hasBomb=true;
    }
    public void ThrowBomb(){//Animation Event
        if (hasBomb&&TargetPoint!=null)
        {
            TargetPoint.SetParent(pickupPonint.parent.parent);
            if (hasBomb)
            {
                TargetPoint.GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Dynamic;
            }
            if (FindObjectOfType<playerControl>().transform.position.x-pickupPonint.position.x<0)
            {
                TargetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1,1)*10,ForceMode2D.Impulse);
            }else
            {
                TargetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,1)*10,ForceMode2D.Impulse);
            }
            
        }
        hasBomb=false;
    }
}
