using UnityEngine;

public class KoloMovement : CharacterBase
{
    public float defaultMovementSpeed = 7.0f;
    public float jumpForce = 10f;
    public float jumpHoldForce = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float dashSpeedMultiplier = 3f;
    public float dashCooldown = 3f;
    public float dashDuration = 0.2f;
    public LayerMask groundLayer;
    public float RaycastLenght = 0.1f;

    // private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 movementInput;
    private float lastDashTime;
    private bool isDashing;
    private float dashEndTime;
    private bool isJumping;
    private bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        movementSpeed = defaultMovementSpeed;
        // animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Получаем вход для движения и прыжка
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Проверка начала прыжка
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
        
        // Проверка удержания прыжка
        if (Input.GetKey(KeyCode.Space) && isJumping && rb2d.velocity.y > 0)
        {
            rb2d.AddForce(Vector2.up * jumpHoldForce * Time.deltaTime, ForceMode2D.Impulse);
        }

        // Проверка завершения прыжка при отпускании пробела
        if (Input.GetKeyUp(KeyCode.Space) && rb2d.velocity.y > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }

        // Проверка нажатия для выполнения дэша
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time - lastDashTime >= dashCooldown)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            lastDashTime = Time.time;
            movementSpeed *= dashSpeedMultiplier;
        }

        // Завершение дэша
        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
            movementSpeed = defaultMovementSpeed;
        }
    }

    private void FixedUpdate()
    {
        // Движение
        rb2d.velocity = new Vector2(movementInput.x * movementSpeed, rb2d.velocity.y);

        // Применение ускорения при падении и быстрого спуска
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    
    }
    private void IsGrounded()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - collider2d.bounds.extents.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, RaycastLenght, groundLayer);
        isGrounded = hit.collider != null;
    }
}