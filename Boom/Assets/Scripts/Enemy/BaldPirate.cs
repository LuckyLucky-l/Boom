using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldPirate : Enemy,IDamageable
{
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
}