using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float maxDistance = 10f;            // Максимальная дальность молнии
    public LineRenderer lineRenderer;          // Линия для визуализации молнии
    public LayerMask targetLayer;              // Слой целей (например, врагов)
    public int numPoints = 30;                 // Количество точек молнии
    public float lightningDuration = 0.1f;     // Время, в течение которого молния полностью видима
    private Vector3[] lightningPoints;         // Массив точек молнии
    private bool isOnCooldown = false;         // Флаг, который указывает, находится ли молния на кулдауне
    private GameObject targetEnemy;            // Ссылка на врага, если молния в него попала

    void Start()
    {
        lightningPoints = new Vector3[numPoints];
    }

    void Update()
    {
        // Проверяем, что молния не на кулдауне, прежде чем активировать
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isOnCooldown)
        {
            FireLightning();
        }
    }

    void FireLightning()
    {
        // Устанавливаем флаг кулдауна в true
        isOnCooldown = true;
        targetEnemy = null;  // Сбрасываем ссылку на цель при новом запуске молнии

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;

        // Рассчитываем направление от игрока к позиции клика
        Vector2 direction = targetPosition - transform.position;

        // Если расстояние до клика больше maxDistance, устанавливаем конец молнии на maxDistance
        if (direction.magnitude > maxDistance)
        {
            targetPosition = transform.position + (Vector3)direction.normalized * maxDistance;
        }

        // Запускаем Raycast для проверки на столкновения с врагом на пути молнии
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, targetLayer);

        // Генерируем случайные промежуточные точки для молнии
        lightningPoints[0] = transform.position;
        lightningPoints[numPoints - 1] = hit.collider != null ? hit.point : targetPosition;

        // Если молния попала во врага, сохраняем ссылку на него
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            targetEnemy = hit.collider.gameObject;
        }

        for (int i = 1; i < numPoints - 1; i++)
        {
            // Генерация случайных точек между началом и концом
            Vector3 randomPoint = Vector3.Lerp(lightningPoints[0], lightningPoints[numPoints - 1], (float)i / (numPoints - 1));
            randomPoint += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0); // Добавление случайного "шумового" смещения
            lightningPoints[i] = randomPoint;
        }

        // Запуск корутины для плавного появления молнии
        StartCoroutine(DisplayLightningWithDelay());
    }

    private IEnumerator DisplayLightningWithDelay()
    {
        // Устанавливаем количество точек в LineRenderer
        lineRenderer.positionCount = 0;

        // Добавляем точки молнии поочередно с плавным появлением
        for (int i = 0; i < numPoints; i++)
        {
            lineRenderer.positionCount = i + 1;            // Увеличиваем количество точек
            lineRenderer.SetPosition(i, lightningPoints[i]); // Устанавливаем текущую точку
            yield return new WaitForSeconds(lightningDuration / numPoints); // Задержка между появлением точек
        }

        // Уничтожаем врага, если он был поражен и последняя точка уже появилась
        if (targetEnemy != null)
        {
            Destroy(targetEnemy);
        }

        // Ждем указанное время перед исчезновением молнии
        yield return new WaitForSeconds(lightningDuration);

        // Отключаем LineRenderer после того, как молния погасла
        lineRenderer.positionCount = 0;

        // Сбрасываем флаг кулдауна
        isOnCooldown = false;
    }
}















