using UnityEngine;

public class PlayerJump : CharacterBase
{
    // Добавляем ссылку на RandomInputSetup
    public RandomInputSetup randomInputSetup; 

    [Header("Параметры прыжка")]
    public float jumpForce = 10f; // Сила прыжка
    public float jumpTime = 0.35f; // Максимальное время прыжка, зависимое от удержания кнопки
    public LayerMask groundLayer; // Слой, представляющий землю
    public Transform groundCheck; // Точка для проверки земли
    public float groundCheckDistance = 0.2f; // Дистанция проверки земли с помощью рейкаста

    private bool isJumping = false;
    private float jumpTimeCounter;
    private bool jumpButtonReleased = true;

    protected override void Start()
    {
        base.Start();

        // Проверка и присвоение ссылки на RandomInputSetup, если она не задана
        if (randomInputSetup == null)
        {
            randomInputSetup = GetComponent<RandomInputSetup>();
            if (randomInputSetup == null)
            {
                Debug.LogError("RandomInputSetup не найден! Пожалуйста, добавьте его на объект игрока.");
                return;
            }
        }
    }

    void Update()
    {
        // Используем случайную клавишу для прыжка
        if (Input.GetKeyDown(randomInputSetup.jumpKey) && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpButtonReleased = false;
        }

        if (Input.GetKey(randomInputSetup.jumpKey) && isJumping)
        {
            if (jumpTimeCounter > 0 && !jumpButtonReleased)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(randomInputSetup.jumpKey))
        {
            isJumping = false;
            jumpButtonReleased = true;
        }
    }

    void FixedUpdate()
    {
        if (IsGrounded() && rb.velocity.y <= 0)
        {
            isJumping = false;
        }

        ApplyGravity();
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}









