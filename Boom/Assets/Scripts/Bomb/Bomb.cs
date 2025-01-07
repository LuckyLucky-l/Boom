using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D cd2;
    public float startTime;
    public float waitTime;
    public float BombForce;
    [Header("Check")]
    public float radius;//检测范围
    public LayerMask targetLayer;
    void Start()
    {
        anim=GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        cd2=GetComponent<Collider2D>();
        startTime=Time.time;
    }

    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bomb_Off"))
        {
            if (Time.time>startTime+waitTime)
            {
             anim.Play("Bomb_Explotion");
            }
        }
    }
    void Explotion(){//爆炸产生的效果
        cd2.enabled=false;
        rb.gravityScale=0;
        //检测周围的对象
        Collider2D[] AllCollider=Physics2D.OverlapCircleAll(this.transform.position,radius,targetLayer);
        //得到方向向量
        //给他们添加力的效果
        foreach (Collider2D item in AllCollider)
        {
            Vector3 pos=this.transform.position-item.transform.position;
            //炸的时候给对方添加一个力
            item.GetComponent<Rigidbody2D>().AddForce(BombForce*(-pos+Vector3.up),ForceMode2D.Impulse);
            //对方是熄灭的炸弹，则点燃它
            if (item.CompareTag("Bomb")&&item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Bomb_Off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }else if (item.CompareTag("Player"))//如果是玩家就炸3滴血
            {
                item.GetComponent<IDamageable>().GetHit(3);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position,radius);
    }
    void DestoryThis(){
        Destroy(gameObject);
    }
    public void TurnOff(){
            anim.Play("Bomb_Off");
            gameObject.layer=LayerMask.NameToLayer("NPC");
    }
    public void TurnOn(){
        anim.Play("Bomb_On");
        startTime=Time.time;
        gameObject.layer=LayerMask.NameToLayer("Bomb");
    }
}
