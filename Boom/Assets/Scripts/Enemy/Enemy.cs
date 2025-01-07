using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 敌人逻辑
/// 1.来回两个点走动，检测到炸弹就吹灭，检测到人就攻击
/// 怎么选择攻击哪个？选择距离更远的
/// </summary>
public class Enemy : MonoBehaviour
{
    private GameObject Sign;
    [Header("Enemy state")]
    public float Healthy;
    public bool isDead;
    public bool hasBomb;
    public bool isBoos;
    EnemyBaseState currentState;//怪物的行为状态机选择
    public Animator anim;
    public int animState;//0=基础动画状态机(待机)，2=攻击动画状态机(待机)
    public PatrolState patrolState=new PatrolState();//循环模式
    public AttackState attackState=new AttackState();
    [Header("Movement")]
    public float speed;
    public Transform PointA,PointB;
    public Transform TargetPoint;
    public List<Transform> AttackList;

    [Header("Attack Setting")]
    private float nextAttack=0;
    public float attackRate;
    public float attackRange,skillRange;

    public virtual void Init(){
        Sign=transform.GetChild(0).gameObject;
        anim=GetComponent<Animator>();  
    }
    void Awake()
    {
        Init();
    }
    void Start()
    {
        GameManager.instance.IsEnemy(this); 
        if (isBoos)
        {
            UIManger.Instance.BoosHealhActive(isBoos);
            UIManger.Instance.SetBoosHealth(Healthy);
        }
        TransformToState(patrolState);//一开始把行为设为巡逻逻辑
        
    }   
   public virtual void Update()
    {
        if (isBoos)
         {
            UIManger.Instance.UpdateBoosHealth(Healthy);
         }
        anim.SetBool("dead",isDead);
        if (isDead)
        {
            if (isBoos)
            {
                MusicManager.instance.BossDeadMusic();
                isBoos=false;
            }
            GameManager.instance.EnemyDead(this);
            transform.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
            transform.GetComponent<SpriteRenderer>().sortingOrder=-1;
            return;
        }
         currentState.OnUpdateState(this);
         anim.SetInteger("state",animState);
    }
    //切换Enemy状态模式
    public void TransformToState(EnemyBaseState state){
        currentState=state;
        currentState.EnterState(this);
    }
    //移动到目标点然后转身
   public void Move(){
        transform.position=Vector2.MoveTowards(transform.position,TargetPoint.position,speed*Time.deltaTime);
        FilpReverse();
    }
    public void SwitchPoint(){//A更远，然后给A
        if (math.abs(transform.position.x-PointA.position.x)>math.abs(transform.position.x-PointB.transform.position.x))
        {
            TargetPoint=PointA;
        }else{
            TargetPoint=PointB;
        }
    }
    void FilpReverse(){
        if (transform.position.x>TargetPoint.position.x)
        {
            transform.rotation=Quaternion.Euler(0,0,0);
        }else{
            transform.rotation=Quaternion.Euler(0,180,0);
        }
    }
    
   public virtual void  Attack(){
        if (Vector2.Distance(transform.position,TargetPoint.position)<attackRange)
        {
            if (Time.time>nextAttack)
            {
                 //播放攻击动画
                 anim.SetTrigger("attack");
                 nextAttack=Time.time+attackRate;
            }
           
        }
    }
   public virtual void SkillAttack(){
    if (Vector2.Distance(transform.position,TargetPoint.position)<skillRange)
        {
            if (Time.time>nextAttack)
            {
                 //播放动画
                 anim.SetTrigger("skillAttack");
                 nextAttack=Time.time+attackRate;
            }
           
        }
   }
   void OnTriggerStay2D(Collider2D other)
    {
            if (!AttackList.Contains(other.transform)&&!hasBomb&&!isDead&&!GameManager.instance.gameOver)
             {
                 AttackList.Add(other.transform);
             }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        AttackList.Remove(other.transform);
    }
   void OnTriggerEnter2D(Collider2D other)
   {
        
        if (!isDead&&!GameManager.instance.gameOver)
        {
                StartCoroutine("OnAlarm");
        }
   }
   IEnumerator OnAlarm(){
        Sign.SetActive(true);
        yield return new WaitForSeconds(Sign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Sign.SetActive(false);
   }
    public void ResetTargetPoint()
    {
        // 发射射线检测前方的墙壁
        RaycastHit2D hitA = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, 1<<LayerMask.NameToLayer("ground")); // 向右发射射线
        RaycastHit2D hitB = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, 1<<LayerMask.NameToLayer("ground")); // 向左发射射线
        // 如果检测到墙壁
        if (hitA.collider != null)
        {
            // 在墙的前面放置 PointA
            Vector3 newPositionA = hitA.point + (hitA.normal * 1); // `offsetFromWall` 控制与墙的距离
            PointA.position = new Vector3(newPositionA.x, transform.position.y, transform.position.z);
        }
        
        if (hitB.collider != null)
        {
            // 在墙的前面放置 PointB
            Vector3 newPositionB = hitB.point + (hitB.normal * 1f); // 同样调整 PointB
            PointB.position = new Vector3(newPositionB.x, transform.position.y, transform.position.z);
        }
    }
    private void OnDrawGizmos()
{
    if (this == null) return;

    // 设置射线颜色
    Gizmos.color = Color.red;

    // 绘制射线 A
    Gizmos.DrawRay(this.transform.position + Vector3.right * 0.5f, Vector2.right *  0.5f);

    // 绘制射线 B
    Gizmos.DrawRay(this.transform.position + Vector3.left * 0.5f, Vector2.left *  0.5f);

    // 检测并绘制命中点 A
    RaycastHit2D hitA = Physics2D.Raycast(this.transform.position + Vector3.right * 0.5f, Vector2.right,  0.5f, 1<<LayerMask.NameToLayer("ground"));
    if (hitA.collider != null)
    {
        Gizmos.color = Color.green; // 命中时改颜色
        Gizmos.DrawSphere(hitA.point, 0.05f); // 显示命中点
    }

    // 检测并绘制命中点 B
    RaycastHit2D hitB = Physics2D.Raycast(this.transform.position + Vector3.left * 0.5f, Vector2.left,  0.5f, 1<<LayerMask.NameToLayer("ground"));
    if (hitB.collider != null)
    {
        Gizmos.color = Color.green; // 命中时改颜色
        Gizmos.DrawSphere(hitB.point, 0.05f); // 显示命中点
    }
}
}

