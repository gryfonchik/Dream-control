using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float maxDistance = 10f;            // Максимальная дальность молнии
    public LineRenderer lineRenderer;          // Линия для визуализации молнии
    public LayerMask targetLayer;              // Слой целей (например, врагов)
    public int numPoints = 30;                 // Количество точек молнии
    public float lightningDuration = 0.1f;     // Время, в течение которого молния полностью видима
    public int damageAmount = 1;               // Урон, наносимый боссу молнией
    private Vector3[] lightningPoints;         // Массив точек молнии

    void Start()
    {
        lightningPoints = new Vector3[numPoints];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) // Запуск молнии по правой кнопке мыши
        {
            FireLightning();
        }
    }

    void FireLightning()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;

        // Рассчитываем направление от игрока к позиции клика
        Vector2 direction = targetPosition - transform.position;

        // Если расстояние до клика больше maxDistance, устанавливаем конец молнии на maxDistance
        if (direction.magnitude > maxDistance)
        {
            targetPosition = transform.position + (Vector3)direction.normalized * maxDistance;
        }

        // Запускаем Raycast для проверки на столкновения с врагами на пути молнии
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, targetLayer);

        // Генерируем случайные промежуточные точки для молнии
        lightningPoints[0] = transform.position;
        lightningPoints[numPoints - 1] = hit.collider != null ? hit.point : targetPosition;

        for (int i = 1; i < numPoints - 1; i++)
        {
            // Генерация случайных точек между началом и концом
            Vector3 randomPoint = Vector3.Lerp(lightningPoints[0], lightningPoints[numPoints - 1], (float)i / (numPoints - 1));
            randomPoint += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0); // Добавление случайного "шумового" смещения
            lightningPoints[i] = randomPoint;
        }

        // Запуск корутины для плавного появления молнии
        StartCoroutine(DisplayLightningWithParticles());

        // Обработка столкновения молнии с врагами
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // Если это обычный враг, уничтожаем его
                Destroy(hit.collider.gameObject);
            }
            else if (hit.collider.CompareTag("Boss"))
            {
                // Если это босс, наносим ему урон
                EnemyHealth bossHealth = hit.collider.GetComponent<EnemyHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(damageAmount);
                }
            }
        }
    }

    private IEnumerator DisplayLightningWithParticles()
    {
        // Устанавливаем количество точек в LineRenderer
        lineRenderer.positionCount = 0;

        // Добавляем точки молнии поочередно с плавным появлением
        for (int i = 0; i < numPoints; i++)
        {
            lineRenderer.positionCount = i + 1; // Увеличиваем количество точек
            lineRenderer.SetPosition(i, lightningPoints[i]); // Устанавливаем текущую точку
            yield return new WaitForSeconds(lightningDuration / numPoints); // Задержка между появлением точек
        }

        // После завершения корутины ждем указанное время перед исчезновением молнии
        yield return new WaitForSeconds(lightningDuration);

        // Отключаем LineRenderer после того, как молния погасла
        lineRenderer.positionCount = 0;
    }
}














