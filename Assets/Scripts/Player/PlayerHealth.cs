using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Player player;
    private GameManager gameManager;

    [SerializeField] private int maxHP = 3;
    [SerializeField] private int currentHP = 3;
    [SerializeField] private GameObject Pop;
    //[SerializeField] private float fallThresholdY = -10f;

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
    //    // ³«»ç ÆÇÁ¤
    //    //if (transform.position.y <= fallThresholdY + 0.01f)
    //    //{
    //    //    Fall();
    //    //}

    //    //Debug.Log($"isDie: {isDie}");
    //    //Debug.Log($"y°ª: {transform.position.y}");
    //}

    // ¸ó½ºÅÍ µ¥¹ÌÁö
    public void TakeDamage(int amount)
    {
        Debug.Log("PlayerHealth: TakeDamage");
        if (currentHP > 1)
        {
            if (player.isDamaged)
            {
                currentHP -= amount;
                Debug.Log($"currentHP: {currentHP}");
            }
        }
        else
        {
            isDie = true;
            Debug.Log($"isDie: {isDie}");
            Instantiate(Pop, transform.position, Quaternion.identity);
            Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    // ³«»ç
    public void Fall()
    {
        Debug.Log("³«»ç");
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
