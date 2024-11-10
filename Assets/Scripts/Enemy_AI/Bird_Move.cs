using UnityEngine;

public class Bird_Move : MonoBehaviour
{
    public float moveSpeed = 2f;          // Скорость перемещения влево
    public float downSpeed = 5f;          // Скорость перемещения вниз после пересечения луча
    public LayerMask playerLayer;         // Слой игрока, чтобы луч не срабатывал на другие объекты
    public Transform raycastPoint;        // Точка запуска Raycast (перетаскивается в Inspector)
    public Rigidbody2D rb;                // Ссылка на Rigidbody2D, назначается в Inspector
    public float raycastLength = 30f;     // Длина луча, задается в Inspector

    private bool isMoving = false;        // Флаг, указывающий, что враг двигается
    private bool isMovingDown = false;    // Флаг, указывающий, что враг движется вниз

    // Этот метод будет вызываться из анимации (с помощью Animation Event)
    public void StartMoving()
    {
        isMoving = true;
        rb.gravityScale = 0;  // Отключаем гравитацию при движении влево
    }

    // Этот метод будет вызываться из анимации для начала движения вниз
    public void StartDownwardMovement()
    {
        isMoving = false;     // Останавливаем движение влево
        isMovingDown = true;  // Начинаем движение вниз
        rb.gravityScale = 1;  // Включаем гравитацию для дальнейших физических расчетов
    }

    private void Start()
    {
        // Отключаем гравитацию на старте, чтобы птица не падала
        rb.gravityScale = 0;
    }

    private void Update()
    {
        if (isMoving && !isMovingDown)
        {
            // Двигаем противника влево по оси X
            MoveLeft();
        }

        if (isMovingDown)
        {
            // Двигаем противника вниз по оси Y
            MoveDown();
        }
    }

    private void MoveLeft()
    {
        // Устанавливаем скорость влево по оси X и фиксируем высоту по оси Y
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // Сохраняем текущее значение Y в скорости

        // Проверка на столкновение с игроком через Raycast от RaycastPoint
        
        RaycastHit2D hit = new RaycastHit2D(); // Инициализация переменной перед использованием
        
        if (raycastPoint != null)
        {
             hit = Physics2D.Raycast(raycastPoint.position, Vector2.down, raycastLength, playerLayer);
        }
        else
        {
            Debug.LogWarning("raycastPoint is not assigned or has been destroyed.");
        }

        // Отображение луча для отладки
        Debug.DrawRay(raycastPoint.position, Vector2.down * raycastLength, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("Player detected, starting downward movement."); // Сообщение, если игрок обнаружен
            isMoving = false;     // Останавливаем движение влево
            isMovingDown = true;  // Начинаем движение вниз
        }
    }

    private void MoveDown()
    {
        // Устанавливаем скорость вниз по оси Y, фиксируя ось X
        rb.velocity = new Vector2(0, -downSpeed);

        // Включаем гравитацию, чтобы птица учитывала физические взаимодействия с ground
        rb.gravityScale = 1;
    }
}














