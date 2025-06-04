using System;
using Unity.Burst.CompilerServices;
using UnityEngine;


public class Player : MonoBehaviour , IDamageable
{
    private PlayerHealth playerHealth;
    public StateMachine stateMachine;

    [SerializeField] private LadderSensor ladderSensor;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Attack")]
    [SerializeField] private Transform enemyCheckPos;
    [SerializeField] private float boxWidth = 0.8f;
    [SerializeField] private float boxHeight = 0.1f;
    [SerializeField] private LayerMask enemyLayer;
    public bool isAttack;
    public static event Action<Player> OnPlayerDamaged;

    [Header("Ladder")]
    public float centerX;

    [Header("Player fields")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpPower;

    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    [Space(10)]
    public const float initialPlayerGravityScale = 3f;
    public float moveInput;
    public float climbInput;
    public int facingDir;
    [Space(10)]
    public bool isJumped;
    public bool isGrounded;
    public bool isLadder;
    public bool isClimbing;
    public bool isDamaged;

    public readonly int IDLE_HASH = Animator.StringToHash("Idle_Fox");
    public readonly int RUN_HASH = Animator.StringToHash("Run_Fox");
    public readonly int JUMP_HASH = Animator.StringToHash("Jump_Fox");
    public readonly int Crouch_HASH = Animator.StringToHash("Crouch_Fox");
    public readonly int CLIMB_HASH = Animator.StringToHash("Climb_Fox");
    public readonly int HURT_HASH = Animator.StringToHash("Hurt_Fox");
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();

        ladderSensor.OnEnter += HandleLadderEnter;
        ladderSensor.OnExit += HandleLadderExit;

        StateMachineInit();
    }

    private void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.playerStateDic.Add(PlayerEState.Idle, new Player_Idle(this));
        stateMachine.playerStateDic.Add(PlayerEState.Run, new Player_Run(this));
        stateMachine.playerStateDic.Add(PlayerEState.Jump, new Player_Jump(this));
        stateMachine.playerStateDic.Add(PlayerEState.Climb, new Player_Climb(this));
        stateMachine.playerStateDic.Add(PlayerEState.Hurt, new Player_Hurt(this));

        stateMachine.CurState = stateMachine.playerStateDic[PlayerEState.Idle];
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        climbInput = Input.GetAxisRaw("Vertical");
        isJumped = Input.GetKeyDown(KeyCode.Space);
        isClimbing = climbInput != 0;

        stateMachine.Update();

        CheckGround();
        CheckEnemy();

        //Debug.Log($"isLadder: {isLadder}");
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void HandleLadderEnter(GameObject collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            isLadder = true;
            centerX = collision.GetComponent<BoxCollider2D>().bounds.center.x;
        }    
    }

    private void HandleLadderExit(GameObject collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayer);
    }

    private void CheckEnemy()
    {
        Collider2D enemy = Physics2D.OverlapBox(enemyCheckPos.position, new Vector2(boxWidth, boxHeight), 0f, enemyLayer);
        IDamageable target = enemy?.GetComponent<IDamageable>();

        if (enemy != null)
        {
            Debug.Log("Enemy π‚¿Ω!");
            target.TakeDamage();
            isAttack = true;
        }
    }

    public void TakeDamage()
    {
        Debug.Log("TakeDamage");

        OnPlayerDamaged?.Invoke(this);
        isDamaged = true;
        playerHealth.TakeDamage(1);
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);
        }

        if (groundCheckPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(enemyCheckPos.position, new Vector2(boxWidth, boxHeight));
        }
    }
}
