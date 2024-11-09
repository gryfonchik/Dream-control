using UnityEngine;

public class Bird_Move : MonoBehaviour
{
    public float moveSpeed = 2f; // Скорость перемещения влево
    public float fallSpeed = 2f; // Начальная скорость падения
    public float fallAcceleration = 0.1f; // Ускорение падения
    public LayerMask playerLayer; // Слой игрока, чтобы луч не срабатывал на другие объекты

    private bool isMoving = false; // Флаг, указывающий, что враг двигается
    private bool isFalling = false; // Флаг, указывающий, что враг падает

    // Этот метод будет вызываться из анимации (с помощью Animation Event)
    public void StartMoving()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Двигаем противника влево по оси X
            MoveLeft();
        }

        // Если враг начинает падать, выполняем логику падения
        if (isFalling)
        {
            Fall();
        }
    }

    private void MoveLeft()
    {
        // Двигаем врага влево бесконечно
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // Проверка на столкновение с игроком через Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, playerLayer);

        if (hit.collider != null)
        {
            // Если луч столкнулся с коллайдером игрока, начинаем падение
            isMoving = false; // Останавливаем движение влево
            isFalling = true; // Начинаем падение
        }
    }

    // Логика падения врага
    private void Fall()
    {
        // Ускоряем падение с течением времени
        fallSpeed += fallAcceleration * Time.deltaTime;
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime); // Падение вниз
    }
}



