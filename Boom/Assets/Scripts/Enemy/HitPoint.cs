using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public bool bombAavilable;
    float dic;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.x<other.transform.position.x)
        {
            dic=1;
        }else{
            dic=-1;
        }
        if (other.CompareTag("Player"))
        {
            //print("玩家被攻击");
            other.GetComponent<IDamageable>().GetHit(1);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(dic,1)*5,ForceMode2D.Impulse);
        }else if (other.CompareTag("Bomb")&& bombAavilable)
        {
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(dic,1)*10,ForceMode2D.Impulse);
        }
    }
}
