using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : CharacterBase
{
    public RandomInputSetup randomInputSetup; // Ссылка на скрипт с назначенными клавишами

    [Header("Основные параметры движения")]
    public float Speed = 5f;

    [Header("Параметры рывка")]
    public float DashSpeed = 20f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;

    private bool _isDashing = false;
    private float _dashTime = 0f;
    private float _nextDashTime = 0f;
    private Vector2 _originalVelocity;

    private Vector2 _movementVector
    {
        get
        {
            float horizontal = 0f;

            // Проверяем нажатие случайных клавиш для движения влево и вправо
            if (Input.GetKey(randomInputSetup.moveRightKey)) // Если нажата клавиша для движения вправо
            {
                horizontal = 1f;
            }
            else if (Input.GetKey(randomInputSetup.moveLeftKey)) // Если нажата клавиша для движения влево
            {
                horizontal = -1f;
            }

            return new Vector2(horizontal, 0.0f);
        }
    }

    protected override void Start()
    {
        base.Start();
        if (gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.LogError("Player SortingLayer must be different from Ground SortingLayer!");
        }
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
        // Проверка нажатия клавиши для выполнения рывка
        if (Input.GetKeyDown(randomInputSetup.dashKey) && Time.time >= _nextDashTime && !_isDashing)
        {
            StartDash();
        }

        // Обновление состояния рывка
        if (_isDashing)
        {
            _dashTime += Time.deltaTime;
            if (_dashTime >= DashDuration)
            {
                EndDash();
            }
        }
    }

    void FixedUpdate()
    {
        if (!_isDashing)
        {
            HandleMovement();
        }
        else
        {
            PerformDash();
        }

        ApplyGravity();  // Используем метод из базового класса для применения гравитации
    }

    private void HandleMovement()
    {
        Vector2 movement = _movementVector;
        if (movement.magnitude > 0)
        {
            rb.velocity = new Vector2(movement.x * Speed, rb.velocity.y); // Здесь заменили _rb на rb
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Здесь тоже заменили _rb на rb
        }
    }

    private void PerformDash()
    {
        rb.velocity = new Vector2(DashSpeed * _movementVector.x, rb.velocity.y); // И здесь заменили _rb на rb
    }

    private void StartDash()
    {
        _isDashing = true;
        _dashTime = 0f;
        _nextDashTime = Time.time + DashCooldown;
        _originalVelocity = rb.velocity;
    }

    private void EndDash()
    {
        _isDashing = false;
        rb.velocity = _originalVelocity;
    }
}













