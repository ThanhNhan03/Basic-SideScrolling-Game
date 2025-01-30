using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; // Để sử dụng Coroutine

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float climbSpeed = 5;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D myFeetCollider;
    GameManager gameManager;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;

    bool isAlive = true;

    float gravityScaleAtStart;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = rigidbody2d.gravityScale;
        myFeetCollider = GetComponent<BoxCollider2D>();

        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        ClimbLadder();
        SlipSprite();
        Die();
    }

    void OnAttack(InputValue value)
    {
        if (!isAlive) { return; }

        if (value.isPressed)
        {
            Instantiate(bullet, firePoint.position, transform.rotation);
            animator.SetTrigger("isAttack");
            Debug.Log("Fire");
        }
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }

        moveInput = value.Get<Vector2>();
        animator.SetBool("isRunning", true);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (value.isPressed)
        {
            rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpForce);
        }
    }

    

    void Die()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard")))
        {
            Debug.Log("Player Died");
            isAlive = false;
            animator.SetTrigger("isDying"); // Bắt đầu animation "isDying"
            rigidbody2d.linearVelocity = deathKick;

            // Chờ cho animation "isDying" hoàn thành trước khi game over
            StartCoroutine(PlayDeathAnimationAndShowGameOver());
        }
    }

    private IEnumerator PlayDeathAnimationAndShowGameOver()
    {
        // Đợi animation "isDying" hoàn thành (giả sử animation có thời gian dài 2 giây)
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Đợi cho animation hoàn thành
        yield return new WaitForSeconds(animationDuration);

        // Sau khi animation hoàn tất, hiển thị màn hình Game Over
        gameManager.GameOver(); // Hiển thị Game Over Panel
        gameObject.SetActive(false); // Tắt nhân vật
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rigidbody2d.linearVelocity.y);
        rigidbody2d.linearVelocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rigidbody2d.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(rigidbody2d.linearVelocity.x, moveInput.y * climbSpeed);
        rigidbody2d.linearVelocity = climbVelocity;
        rigidbody2d.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(rigidbody2d.linearVelocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void SlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.linearVelocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2d.linearVelocity.x), 1f);
    }
}
