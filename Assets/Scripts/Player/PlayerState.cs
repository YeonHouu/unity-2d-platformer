using UnityEngine;

public enum PlayerEState
{
    Idle, Run, Jump, Crouch, Climb, Hurt, Attack
}

public class PlayerState : BaseState
{
    protected Player player;

    public PlayerState(Player _player)
    {
        player = _player;
    }

    public override void Enter()

    {
        player.animator.speed = 1f;
    }

    public override void Update()
    {
        // Jump
        if ((player.isJumped && player.isGrounded) || (player.isJumped && player.isLadder))
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Jump]);
        }

        // Climb
        if (player.isClimbing && player.isLadder)
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Climb]);
        }

        // Hurt
        if(player.isDamaged)
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Hurt]);
        }
    }

    public override void Exit() { }

}

#region Idle
public class Player_Idle : PlayerState
{
    public Player_Idle(Player _player) : base(_player)
    {
        hasPhysics = false;
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Idle Enter");
        player.animator.Play(player.IDLE_HASH);
        player.rigid.velocity = Vector3.zero;
    }

    public override void Update()
    {
        base.Update();

        if (Mathf.Abs(player.moveInput) != 0)
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Run]);
        }
    }
    public override void Exit() { }
}
#endregion

#region Run
public class Player_Run : PlayerState
{
    public Player_Run(Player _player) : base(_player)
    {
        hasPhysics = true;
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Run Enter");
        player.animator.Play(player.RUN_HASH);
    }

    public override void Update()
    {
        base.Update();

        if (Mathf.Abs(player.moveInput) == 0)
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Idle]);
        }

        if (player.moveInput < 0)
        {
            player.facingDir = -1;
            player.spriteRenderer.flipX = true;
        }
        else
        {
            player.facingDir = 1;
            player.spriteRenderer.flipX = false;
        }


    }

    public override void FixedUpdate()
    {
        player.rigid.velocity = new Vector2(player.moveInput * player.moveSpeed, player.rigid.velocity.y);
    }
}
#endregion

#region Jump
public class Player_Jump : PlayerState
{
    private float jumpTimer;

    public Player_Jump(Player _player) : base(_player)
    {
        hasPhysics = true;
    }

    public override void Enter()
    {
        //Debug.Log("Jump Enter");
        jumpTimer = 0;
        player.animator.Play(player.JUMP_HASH);
        player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);
        player.isJumped = false;
        player.isGrounded = false;
        player.isAttacked = false;
    }

    public override void Update()
    {
        base.Update();
        EnemyHit();

        jumpTimer += Time.deltaTime;

        // 최소 점프 시간 보장
        if (jumpTimer > 0.05f && player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Idle]);
        }

        if (player.moveInput < 0)
        {
            player.facingDir = -1;
            player.spriteRenderer.flipX = true;
        }
        else
        {
            player.facingDir = 1;
            player.spriteRenderer.flipX = false;
        }
    }
    public override void FixedUpdate()
    {
        player.rigid.velocity = new Vector2(player.moveInput * player.moveSpeed, player.rigid.velocity.y);
    }

    private void EnemyHit()
    {
        if (player.isAttacked)
        {
            player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.jumpPower);
            Debug.Log("점프 반동 점프");
            player.isAttacked = false;
        }
    }
}
#endregion

#region Climb
public class Player_Climb : PlayerState
{
    public Player_Climb(Player _player) : base(_player)
    {
        hasPhysics = true;
    }

    public override void Enter()
    {
        //Debug.Log("Climb Enter");
        player.animator.Play(player.CLIMB_HASH);

        player.isJumped = false;
        player.isGrounded = false;

        player.rigid.gravityScale = 0f;
        player.transform.position = new Vector2(player.centerX, player.transform.position.y);
        player.rigid.velocity = new Vector2(0f, 0f);
    }

    public override void Update()
    {
        base.Update();

        // climbing 중 움직임 멈추면 애니메이션 stop
        if (player.climbInput == 0)
        {
            player.animator.speed = 0;
        }
        else
        {
            player.animator.speed = 1f;
        }

        // Idle
        if (!player.isLadder)
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Idle]);
        }
    }

    public override void FixedUpdate()
    {

        player.rigid.velocity = new Vector2(player.rigid.velocity.x, player.climbInput * player.moveSpeed);
    }

    public override void Exit()
    {
        Debug.Log("Climb Exit");
        player.animator.speed = 1f;
        player.isClimbing = false;
        player.rigid.gravityScale = Player.initialPlayerGravityScale;
    }
}
#endregion

#region Hurt
public class Player_Hurt : PlayerState
{
    public Player_Hurt(Player _player) : base(_player)
    {
        hasPhysics = false;
    }

    public override void Enter()
    {
        Debug.Log("Hurt Enter");
        player.isDamaged = false;
        player.animator.Play(player.HURT_HASH);
        player.rigid.velocity = Vector2.zero;
        player.rigid.AddForce(new Vector2(-player.facingDir * 10f, 10f), ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();
        //player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Idle]);
    }

    public override void Exit()
    {

    }
}
#endregion