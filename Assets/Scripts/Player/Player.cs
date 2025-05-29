using UnityEngine;


public class Player : MonoBehaviour
{
    public StateMachine stateMachine;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

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
    [Space(10)]
    public bool isJumped;
    public bool isGrounded;
    public bool isLadder;
    public bool isClimbing;

    public readonly int IDLE_HASH = Animator.StringToHash("Idle_Fox");
    public readonly int Run_HASH = Animator.StringToHash("Run_Fox");
    public readonly int Jump_HASH = Animator.StringToHash("Jump_Fox");
    public readonly int Crouch_HASH = Animator.StringToHash("Crouch_Fox");
    public readonly int Climb_HASH = Animator.StringToHash("Climb_Fox");
    public readonly int Hurt_HASH = Animator.StringToHash("Hurt_Fox");
    
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StateMachineInit();
    }

    private void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.playerStateDic.Add(PlayerEState.Idle, new Player_Idle(this));
        stateMachine.playerStateDic.Add(PlayerEState.Run, new Player_Run(this));
        stateMachine.playerStateDic.Add(PlayerEState.Jump, new Player_Jump(this));
        stateMachine.playerStateDic.Add(PlayerEState.Climb, new Player_Climb(this));

        stateMachine.CurState = stateMachine.playerStateDic[PlayerEState.Idle];
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        climbInput = Input.GetAxisRaw("Vertical");
        isJumped = Input.GetKeyDown(KeyCode.Space);
        isClimbing = climbInput != 0;

        stateMachine.Update();
        Debug.Log($"isGrounded : {isGrounded}");
        //Debug.Log($"isLadder : {isLadder}");
        //Debug.Log($"gravityScale : {rigid.gravityScale}");
        //Debug.Log($"isClimbing : {isClimbing}");

        CheckGround();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ladder"))
        {
            isLadder = true;
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isLadder = false;
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);
        }
    }
}
