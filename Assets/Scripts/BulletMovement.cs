using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float speed = 20f;
    [SerializeField] float lifeTime = 2f;
    Rigidbody2D rb;
    PlayerMovement player;
    float xSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * speed;

        // Lật viên đạn dựa trên hướng của player
        Vector3 bulletScale = transform.localScale;
        bulletScale.x = Mathf.Sign(player.transform.localScale.x);
        transform.localScale = bulletScale;
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(xSpeed, 0);
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemies"))
        {
            Animator enemyAnimator = collision.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Die");
            }
            Destroy(collision.gameObject, 0.5f); // Delay để animation chạy xong
        }
        Destroy(gameObject);
    }
}