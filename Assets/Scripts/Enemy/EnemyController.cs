using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask groundLayer;

    public static event Action<EnemyController> OnEnemyDamaged;

    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 walkVec;
    private bool isWaited;

    private readonly int IDLE_HASH = Animator.StringToHash("bunnyAnimation");
    private readonly int WALK_HASH = Animator.StringToHash("bunnyAnimation");
    private readonly int DIE_HASH = Animator.StringToHash("Enemy_Death");

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkVec = Vector2.right;
    }

    void Update()
    {
        Patrol();
    }

    private void FixedUpdate()
    {
        if (isWaited == false)
            rigid.velocity = walkVec * moveSpeed;
    }

    private void Patrol()
    {
        Vector2 rayOrigin = transform.position + new Vector3(walkVec.x, 0);
        Debug.DrawRay(rayOrigin, Vector2.down * 3f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 3f, groundLayer);
        if (hit.collider == null)
        {
            StartCoroutine(CoTurnBack());
        }
    }

    private IEnumerator CoTurnBack()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;

        if (spriteRenderer.flipX)
        {
            walkVec = Vector2.left;
        }
        else
        {
            walkVec = Vector2.right;
        }

        animator.Play(IDLE_HASH);
        isWaited = true;
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);
        isWaited = false;

        animator.Play(WALK_HASH);
    }

    public void TakeDamage()
    {
        OnEnemyDamaged?.Invoke(this);
        //Debug.Log($"{gameObject.name} 공격받음");

        animator.Play(DIE_HASH);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
        //Debug.Log($"{gameObject.name} 삭제");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            target?.TakeDamage();
            Debug.Log("Player와 충돌!");
        }
    }
}
