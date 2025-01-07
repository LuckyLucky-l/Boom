using System;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    private float time=0;
    public override void EnterState(Enemy enemy)
    {
        enemy.SwitchPoint();//一开始选择去哪A还是B巡逻
        enemy.animState=0;
    }

    public override void OnUpdateState(Enemy enemy)
    {
        time+=Time.deltaTime;
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idel"))//如果当前不在待机动画，就切换跑步动画
        {
            enemy.animState=1;
            enemy.Move(); 
        }
       if (MathF.Abs(enemy.transform.position.x-enemy.TargetPoint.position.x)<0.01f)//如果到了目标点就切换目标点
        {
            enemy.TransformToState(enemy.patrolState);
        }
        RaycastHit2D hitA = Physics2D.Raycast(enemy.transform.position + Vector3.right * 0.5f, Vector2.right, 0.5f, 1 << LayerMask.NameToLayer("ground"));
        RaycastHit2D hitB = Physics2D.Raycast(enemy.transform.position + Vector3.left * 0.5f, Vector2.left, 0.5f, 1 << LayerMask.NameToLayer("ground"));
        if (hitA.collider!=null||hitB.collider!=null)//如果碰到障碍物就切换目标点
        {
            enemy.ResetTargetPoint();
        }
        if (enemy.AttackList.Count>0)//如果有敌人就切换到攻击模式
        {
            enemy.TransformToState(enemy.attackState);
        }
    }
}

