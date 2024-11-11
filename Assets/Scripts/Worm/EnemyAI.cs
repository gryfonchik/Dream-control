using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player; // Игрок, за которым будет следовать противник
    public float moveSpeed = 2f; // Скорость перемещения противника
    private Vector3 targetPosition; // Целевая позиция для движения

    private Renderer enemyRenderer; // Renderer врага для изменения альфа-канала
    private Color startColor; // Начальный цвет (прозрачный)
    private bool isVisible = false; // Флаг, когда враг становится видимым
    private bool isFacingRight = true; // Флаг, указывающий, смотрит ли враг вправо

    private float visibilityTime = 3f; // Время на полное проявление врага
    private float timer = 0f; // Таймер для отслеживания времени

    private Animator animator;

    void Start()
    {
        // Находим объект с тегом "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform; // Присваиваем его трансформ в переменную
        }

        // Получаем Renderer врага (если это SpriteRenderer, это подойдет для 2D-игры)
        enemyRenderer = GetComponent<Renderer>();

        animator = GetComponent<Animator>(); // Получаем Animator

        // Сохраняем начальный цвет объекта
        startColor = enemyRenderer.material.color;

        // Устанавливаем начальную прозрачность (невидимый)
        Color transparentColor = startColor;
        transparentColor.a = 0f; // Устанавливаем альфа-канал в 0 для полной прозрачности
        enemyRenderer.material.color = transparentColor;
    }

    void Update()
    {
        // Увеличиваем альфа-канал с течением времени
        if (timer < visibilityTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / visibilityTime);
            Color currentColor = startColor;
            currentColor.a = alpha;
            enemyRenderer.material.color = currentColor;

            if (timer >= visibilityTime)
            {
                isVisible = true; // Враг стал видимым
                animator.applyRootMotion = true; // Включаем анимацию
            }
        }

        // Если враг стал видимым, начинаем движение
        if (isVisible && player != null)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        // Вычисляем позицию игрока относительно противника
        if (player.position.x > transform.position.x)
        {
            // Игрок справа, двигаемся вправо
            targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            Flip(false); // Повернуть вправо
        }
        else if (player.position.x < transform.position.x)
        {
            // Игрок слева, двигаемся влево
            targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            Flip(true); // Повернуть влево
        }

        // Двигаемся к позиции игрока
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void Flip(bool faceRight)
    {
        if (isFacingRight != faceRight)
        {
            isFacingRight = faceRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1; // Инвертируем по оси X
            transform.localScale = scale;
        }
    }
}