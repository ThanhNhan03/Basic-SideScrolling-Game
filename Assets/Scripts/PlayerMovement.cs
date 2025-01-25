using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector2 moveInput;
    Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 5;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D; 
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        SlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
        animator.SetBool("isRunning", true);
    }

    void OnJump(InputValue value)
    {
        if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (value.isPressed)
        {
            rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocity.x, jumpForce);
     h
            Debug.Log("Jump");
        }

    }

  

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rigidbody2d.linearVelocity.y);
        rigidbody2d.linearVelocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }


    void SlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.linearVelocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2d.linearVelocity.x), 1f);
    }
}
