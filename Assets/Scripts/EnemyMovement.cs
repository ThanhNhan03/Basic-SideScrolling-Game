using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float groundCheckDistance = 1f; // Khoảng cách để kiểm tra nền đất
    [SerializeField] LayerMask groundLayer; // Lớp nền đất để Raycast kiểm tra
    Rigidbody2D rigidbody2d;
   

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Enemy di chuyển
        rigidbody2d.linearVelocity = new Vector2(moveSpeed, 0f);

        // Kiểm tra phía trước có nền đất không
        if (!IsGroundInFront())
        {
            // Đổi hướng khi không có nền đất
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Đổi hướng khi va chạm với vật cản
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        // Lật hướng enemy
        transform.localScale = new Vector2(-Mathf.Sign(rigidbody2d.linearVelocity.x), 1f);
    }

    bool IsGroundInFront()
    {
        // Tạo điểm kiểm tra từ vị trí hiện tại của enemy
        Vector2 origin = new Vector2(transform.position.x + Mathf.Sign(moveSpeed) * 0.5f, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        // Debug Raycast để kiểm tra
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.red);

        // Trả về true nếu có nền đất
        return hit.collider != null;
    }
}
