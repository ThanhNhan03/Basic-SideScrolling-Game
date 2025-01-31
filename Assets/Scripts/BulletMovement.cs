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

            // Vô hiệu hóa Collider và Rigidbody
            Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.simulated = false;
            }

            Collider2D enemyCollider = collision.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            Destroy(collision.gameObject, 0.5f);
        }

        Destroy(gameObject);
    }

}