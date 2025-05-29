using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpPower;

    public StateMachine stateMachine;

    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public const float initialPlayerGravityScale = 3f;
    public float moveInput;
    public float climbInput;
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
        Debug.Log($"isLadder : {isLadder}");
        Debug.Log($"gravityScale : {rigid.gravityScale}");
        Debug.Log($"isClimbing : {isClimbing}");

    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
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
}
