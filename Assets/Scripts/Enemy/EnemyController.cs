using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 walkVec;
    private bool isWaited;

    private readonly int IDLE_HASH = Animator.StringToHash("bunnyAnimation");
    private readonly int WALK_HASH = Animator.StringToHash("bunnyAnimation");

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

    private void FixedUpdate()
    {
        if (isWaited == false)
            rigid.velocity = walkVec * moveSpeed;
    }
}
