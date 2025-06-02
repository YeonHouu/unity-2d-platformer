using UnityEngine;

public enum EnemyEState
{
    Idle, Walk, Die
}

public class EnemyState : BaseState
{
    protected Enemy enemy;

    public EnemyState(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public override void Enter() { }

    public override void Update() { }

    public override void Exit() { }
}

public class Enemy_Idle : EnemyState
{
    private float waitedTime;

    public Enemy_Idle(Enemy _enemy) : base(_enemy)
    {
        hasPhysics = false;
    }

    public override void Enter()
    {
        Debug.Log("Idle Enter");
        enemy.rigid.velocity = Vector3.zero;
        enemy.animator.Play(enemy.IDLE_HASH);
        enemy.spriteRenderer.flipX = !enemy.spriteRenderer.flipX;
        if (enemy.spriteRenderer.flipX)
        {
            enemy.walkVec = Vector2.left;
        }
        else
        {
            enemy.walkVec = Vector2.right;
        }
        waitedTime = 0;
    }

    public override void Update()
    {
        waitedTime += Time.deltaTime;
        if (waitedTime > 3)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.enemyStateDic[EnemyEState.Walk]);
        }
    }
}

public class Enemy_Walk : EnemyState
{
    public Enemy_Walk(Enemy _enemy) : base(_enemy)
    {
        hasPhysics = true;
    }

    public override void Enter()
    {
        Debug.Log("Walk Enter");
        enemy.animator.Play(enemy.WALK_HASH);
    }

    public override void Update()
    {
        Vector2 rayOrigin = enemy.transform.position + new Vector3(enemy.walkVec.x, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 3f, enemy.groundLayer);
        
        Debug.Log($"Raycast Hit: {hit.collider.name}");

        Debug.DrawRay(rayOrigin, Vector2.down * 3f, Color.red);
        if (hit.collider == null)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.enemyStateDic[EnemyEState.Idle]);
        }
    }

    public override void FixedUpdate()
    {
        enemy.rigid.velocity = enemy.walkVec * enemy.moveSpeed;
    }

    public override void Exit() { }
}

public class Enemy_Die : EnemyState
{
    public Enemy_Die(Enemy _enemy) : base(_enemy)
    {
        hasPhysics = false;
    }

    public override void Enter()
    {

    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
