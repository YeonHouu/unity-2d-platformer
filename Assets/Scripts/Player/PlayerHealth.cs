using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Player player;
    private GameManager gameManager;

    [SerializeField] private int maxHP = 3;
    //[SerializeField] private float fallThresholdY = -10f;

    private int currentHP;
    private bool isDie = false;
    public bool IsDead() => isDie;


    private void Awake()
    {
        player = GetComponent<Player>();
        gameManager = FindObjectOfType<GameManager>();
        currentHP = maxHP;
    }

    //private void Update()
    //{
    //    // ���� ����
    //    //if (transform.position.y <= fallThresholdY + 0.01f)
    //    //{
    //    //    Fall();
    //    //}

    //    //Debug.Log($"isDie: {isDie}");
    //    //Debug.Log($"y��: {transform.position.y}");
    //}

    // ���� ������
    public void TakeDamage(int amount)
    {
        if (currentHP > 0)
        {
            if (!player.isDamaged)
            {
                currentHP -= amount;
            }
        }
        else
        {
            isDie = true;
            gameManager.GameOver();
        }
    }

    // ����
    public void Fall()
    {
        Debug.Log("����");
        isDie = true;
        gameManager.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            Fall();
        }
    }
}
