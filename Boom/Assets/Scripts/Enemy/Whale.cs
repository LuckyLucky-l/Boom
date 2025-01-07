using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Enemy,IDamageable
{
    public float scale;//缩放大小
    public List<Transform> Bomb=new List<Transform>();//炸弹列表
    public void GetHit(float damage)
    {
        // if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Player_Hit"))
        // {
            Healthy-=damage;
            if (Healthy<=0)
            {
                Healthy=0;
                isDead=true;
                anim.SetBool("dead",true);
                RestartBomb();
            }
                anim.SetTrigger("hit");
        // }
        
    }
    //动画触发
    public void Swalow(){//吃炸弹
        if (TargetPoint!=null)
            {
                TargetPoint.GetComponent<Bomb>().TurnOff();
                TargetPoint.gameObject.SetActive(false);
                Bomb.Add(TargetPoint);
                transform.localScale*=scale;
            }
        if (Bomb.Count==2)
        {
            //如果已经是最大的大小，则不再吃自己爆炸
            Debug.LogError("已经是最大的大小了");
            GetHit(Healthy);
        }
    }
    public void RestartBomb(){//重启炸弹
        foreach (Transform item in Bomb)
        {
            if (item!=null)
            {
                item.position=transform.position;
                item.GetComponent<Bomb>().startTime=Time.time;
                item.gameObject.SetActive(true);
                item.GetComponent<Rigidbody2D>().AddForce(new Vector2((Random.Range(0, 2) == 0) ? -1 : 1,1)*10,
                                                        ForceMode2D.Impulse);
            }
        }
    }
}
