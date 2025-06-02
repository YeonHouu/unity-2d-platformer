using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public LayerMask groundLayer;

    public StateMachine stateMachine;

    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Vector2 walkVec;
    public bool isWaited;

    public readonly int IDLE_HASH = Animator.StringToHash("bunnyAnimation");
    public readonly int WALK_HASH = Animator.StringToHash("bunnyAnimation");
    public readonly int DIE_HASH = Animator.StringToHash("bunnyAnimation");

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkVec = Vector2.right;

        StateMachineInit();
    }
    private void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.enemyStateDic.Add(EnemyEState.Idle, new Enemy_Idle(this));
        stateMachine.enemyStateDic.Add(EnemyEState.Walk, new Enemy_Walk(this));
        stateMachine.enemyStateDic.Add(EnemyEState.Die, new Enemy_Die(this));

        stateMachine.CurState = stateMachine.enemyStateDic[EnemyEState.Walk];
        Debug.Log($"{stateMachine.CurState}");
        Debug.Log("CurState ¼¼ÆÃ");
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}
