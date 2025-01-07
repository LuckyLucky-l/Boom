using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState=2;
        enemy.TargetPoint=enemy.AttackList[0];
    }

    public override void OnUpdateState(Enemy enemy)
    {
        if (enemy.hasBomb)
        {
            return;
        }
        if (enemy.AttackList.Count==0)
        {
            enemy.TransformToState(enemy.patrolState);
        }
        if (enemy.AttackList.Count>1)
        {
            for (int i = 0; i < enemy.AttackList.Count; i++)
            {
                if (enemy.TargetPoint==null)
                {
                    enemy.TargetPoint=enemy.AttackList[0];
                }
                if (Mathf.Abs(enemy.transform.position.x-enemy.AttackList[i].position.x)<
                    Mathf.Abs(enemy.transform.position.x-enemy.TargetPoint.position.x))
                {
                    enemy.TargetPoint=enemy.AttackList[i];
                }
                
            }
          
        }
        if (enemy.AttackList.Count==1)
        {
            {
                enemy.TargetPoint=enemy.AttackList[0];
            }
        }
        if (enemy.TargetPoint!=null)
        {
            if (enemy.TargetPoint.CompareTag("Player"))
             {
                enemy.Attack();
             }if (enemy.TargetPoint.CompareTag("Bomb"))
             {
                enemy.SkillAttack();
             }
            enemy.Move();
        }
    }
}
