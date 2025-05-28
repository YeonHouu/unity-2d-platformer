using UnityEngine;

public enum PlayerEState
{
    Idle, Run, Jump, Crouch, Climb, Hurt
}

public class PlayerState : BaseState
{
    protected Player player;

    public PlayerState(Player _player)
    {
        player = _player;
    }

    public override void Enter() { }

    public override void Update()
    {
        if (player.isJumped && player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Jump]);
        }
    }

    public override void Exit() { }
}

public class Player_Idle : PlayerState
{
    public Player_Idle(Player _player) : base(_player)
    {
        hasPhysics = false;
    }

    public override void Enter()
    {
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

public class Player_Walk : PlayerState
{
    public Player_Walk(Player _player) : base(_player)
    {
        hasPhysics = true;
    }

    public override void Enter()
    {
        player.animator.Play(player.Run_HASH);
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
            player.spriteRenderer.flipX = true;

        }
        else
        {
            player.spriteRenderer.flipX = false;

        }
    }

    public override void FixedUpdate()
    {
        player.rigid.velocity = new Vector2(player.moveInput * player.moveSpeed, player.rigid.velocity.y);
    }

    public override void Exit() { }
}

public class Player_Jump : PlayerState
{
    public Player_Jump(Player _player) : base(_player)
    {
        hasPhysics = true;
    }

    public override void Enter()
    {
        player.animator.Play(player.Jump_HASH);
        Debug.Log("Jump Enter");
        player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);
        player.isJumped = false;
        player.isGrounded = false;
    }

    public override void Update()
    {
        Debug.Log("Jump Update");

        if (player.isGrounded)
        {
            player.stateMachine.ChangeState(player.stateMachine.playerStateDic[PlayerEState.Idle]);
        }

        if (player.moveInput < 0)
        {
            player.spriteRenderer.flipX = true;

        }
        else
        {
            player.spriteRenderer.flipX = false;
        }
    }
    public override void FixedUpdate()
    {
        player.rigid.velocity = new Vector2(player.moveInput * player.moveSpeed, player.rigid.velocity.y);
    }

    public override void Exit() { }
}