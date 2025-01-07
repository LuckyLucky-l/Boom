using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerControl : MonoBehaviour,IDamageable
{
    private FixedJoystick Joystick;
    [Header("Player State")]
    public float Healthy;
    public bool isDead;
    
    private Animator anim;

    public float moveSpeed;
    public float JumpForce;
    private Rigidbody2D rb;
    [Header("Ground Check")]
    public Transform checkPoint;
    public float radius;
    public LayerMask GroundLayer;
    [Header("States Check")]
    public bool canJump;
    public bool isGround;
    public bool isJump;
    public bool isPlatform;
    [Header("Jump FX")]
    public GameObject jumpFx;
    public GameObject landFX;
    [Header("Jump Setting")]
    [SerializeField]private float JumpCount=0;
    [SerializeField]private float JumpMaxCount=2;
    [Header("Attack Setting")]
    public GameObject BombPrefab;
    public float nextAttack=0;
    public float attackRate;//攻击的频率

    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        Joystick=FindObjectOfType<FixedJoystick>();
        GameManager.instance.IsPlayer(this);
        Healthy=GameManager.instance.LoadHealth();
        UIManger.Instance.changeHp(Healthy);
    }
    void Update()
    {
        anim.SetBool("Dead",isDead);
        if (isDead)
        {
            return;
        }
        CheckJump();
    }
    void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity=Vector2.zero;
            return;
        }
        PhysicCheck();
        move();
        Jump();
    }
   //移动
   void move(){
        //按键控制
        // float HorizontalInput=Input.GetAxisRaw("Horizontal");
        float HorizontalInput=Joystick.Horizontal;
        rb.velocity=new Vector2(moveSpeed*HorizontalInput,rb.velocity.y);

        // if (HorizontalInput!=0)按键控制
        // {
        //     rb.transform.localScale=new Vector3(HorizontalInput,1,1);
        // }
        if (HorizontalInput>0)
        {
            transform.eulerAngles=new Vector3(0,0,0);
        }else if (HorizontalInput<0)
        {
            transform.eulerAngles=new Vector3(0,180,0);
        }
   }
   //跳跃
   void Jump(){
    if (canJump && JumpCount<JumpMaxCount)
    {
        rb.velocity=new Vector2(rb.velocity.x,JumpForce);
        isJump=true;
        rb.gravityScale=4;
        jumpFx.SetActive(true);
        jumpFx.transform.position=this.transform.position+new Vector3(0,-0.54f,0);
        canJump=false;
        JumpCount++;
    }else if (!isGround)
    {
        rb.gravityScale=4;
    }
   }
   public void ButtonJump(){//按下按钮就可以跳起来了
        canJump=true;
   }
   //检测跳跃键
   void CheckJump(){
    if (Input.GetButtonDown("Jump")&&isGround)
    {
        canJump=true;
    }
    if (Input.GetKeyDown(KeyCode.J))
    {
        Attack();
    }
   }
   void PhysicCheck(){
        isGround=Physics2D.OverlapCircle(checkPoint.position,radius,GroundLayer);
        if (!isGround)
        {
            rb.gravityScale=1;
            isJump=false;
        }else{
            JumpCount=0;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkPoint.position,radius);
    }
    void LandFX(){//落地特效
        landFX.SetActive(true);
        landFX.transform.position=this.transform.position+new Vector3(0,-0.75f,0);
    }
    public void Attack(){
        if (Time.time>nextAttack)
        {
            Instantiate<GameObject>(BombPrefab,transform.position,BombPrefab.transform.rotation);
            nextAttack=Time.time+attackRate;
        }
    }

    public void GetHit(float damage)
    {
        Healthy-=damage;
        if (Healthy<=0)
        {
            Healthy=0;
            isDead=true;
            anim.SetBool("Dead",true);          
        }
        PlayerPrefs.SetFloat("PlayerHealth",Healthy);
        UIManger.Instance.changeHp(Healthy);
        anim.SetTrigger("hit");
    }
}

