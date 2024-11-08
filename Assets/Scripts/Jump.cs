using UnityEngine;

// Класс для управления прыжком игрока
public class Jump : CharacterBase
{
    private bool isGrounded;

    // Уникальная скорость прыжка
    public float defaultJumpSpeed = 7.0f;
    public LayerMask groundLayer;
    public float RaycastLenght = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        movementSpeed = defaultJumpSpeed;
    }

    private void CheckGround()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - collider2d.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, RaycastLenght, groundLayer);
        isGrounded = hit.collider != null;
    }

    private void TryJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb2d.AddForce(new Vector2(0, movementSpeed), ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        TryJump();
    }

    void FixedUpdate()
    {
        CheckGround();
    }
}

