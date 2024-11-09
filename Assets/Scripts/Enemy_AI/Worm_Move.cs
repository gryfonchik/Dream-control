using UnityEngine;

public class Worm_Move : MonoBehaviour
{
    public float moveSpeed = 2f; // Скорость перемещения
    public Transform target; // Точка, до которой нужно двигаться (триггер активатора)
    private Animator enemyAnimator; // Ссылка на Animator

    private bool isMoving = false; // Флаг, указывающий, что враг двигается
    private bool hasReachedTarget = false; // Флаг, указывающий, что враг достиг триггера
    private float distanceTraveled = 0f; // Пройденное расстояние
    private float halfDistanceToTarget; // Половина начального расстояния до триггера

    private Vector3 startPosition; // Начальная позиция врага

    // Этот метод будет вызываться из анимации (с помощью Animation Event)
    public void StartMoving()
    {
        isMoving = true;
        startPosition = transform.position; // Сохраняем начальную позицию
        enemyAnimator = GetComponent<Animator>(); // Получаем Animator компонента объекта
    }

    private void Update()
    {
        if (isMoving)
        {
            // Двигаем противника влево по оси X
            MoveLeft();
        }
    }

    private void MoveLeft()
    {
        if (target != null)
        {
            // Двигаем врага только по оси X к триггеру
            Vector3 targetPosition = target.position;
            targetPosition.y = transform.position.y; // Сохраняем текущую высоту врага, двигаем только по X

            if (!hasReachedTarget)
            {
                // Двигаем врага в сторону цели
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Проверка, достиг ли враг цели (когда его позиция близка к триггеру)
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    hasReachedTarget = true; // Враг достиг цели
                    // Вычисляем половину начального расстояния
                    halfDistanceToTarget = Vector3.Distance(startPosition, target.position) / 4f * 2f;
                }
            }

            if (hasReachedTarget)
            {
                // Враг продолжает двигаться влево после достижения цели
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

                // Увеличиваем пройденное расстояние
                distanceTraveled += moveSpeed * Time.deltaTime;

                // Проверка, если враг прошел половину начального расстояния
                if (distanceTraveled >= halfDistanceToTarget)
                {
                    isMoving = false; // Останавливаем движение после достижения половины расстояния
                    // Активируем анимацию, что движение завершено
                    enemyAnimator.SetTrigger("Move_Is_Finished");
                    
                }
            }
        }
        else
        {
            // Если триггер не назначен, враг просто движется влево по оси X
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
    }
}














