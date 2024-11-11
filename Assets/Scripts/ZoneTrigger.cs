using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public Transform player; // Игрок
    public Camera mainCamera; // Камера
    public GameObject enemyPrefab; // Префаб врага
    public Transform spawnPoint; // Точка появления врага
    private bool isCameraLocked = false; // Флаг фиксации камеры
    private CameraFollow cameraFollowScript; // Ссылка на скрипт CameraFollow
    private bool hasTriggered = false; // Флаг, чтобы выполнить действие только один раз
    private GameObject spawnedEnemy; // Переменная для отслеживания заспавненного врага

    private void Start()
    {
        // Получаем ссылку на скрипт CameraFollow на камере
        cameraFollowScript = mainCamera.GetComponent<CameraFollow>();
    }

    private void Update()
    {
        // Проверяем, если враг был уничтожен, разблокируем камеру и границы
        if (isCameraLocked && spawnedEnemy == null)
        {
            UnlockCamera();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если игрок входит в зону и триггер не был активирован, фиксируем камеру и спавним врага
        if (other.CompareTag("Player") && !isCameraLocked && !hasTriggered)
        {
            LockCamera();
            SpawnEnemy();
            hasTriggered = true; // Устанавливаем, что триггер сработал
        }
    }

    private void LockCamera()
    {
        isCameraLocked = true;

        // Отключаем слежение и фиксируем камеру
        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = false;
        }

        // Позиционируем камеру на уровне триггера
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        Vector3 newCameraPosition = mainCamera.transform.position;
        newCameraPosition.x = transform.position.x + cameraHalfWidth; // Меняем только X координату
        mainCamera.transform.position = newCameraPosition;

        // Включаем коллайдеры у всех объектов с тегом "Boundary"
        EnableBoundaryColliders(true);
    }

    private void UnlockCamera()
    {
        isCameraLocked = false;

        // Включаем слежение за игроком
        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = true;
        }

        // Отключаем коллайдеры у всех объектов с тегом "Boundary"
        EnableBoundaryColliders(false);
    }

    private void EnableBoundaryColliders(bool enable)
    {
        // Находим все объекты с тегом "Boundary" и включаем/выключаем их коллайдеры
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");

        foreach (var boundary in boundaries)
        {
            BoxCollider2D boundaryCollider = boundary.GetComponent<BoxCollider2D>();
            if (boundaryCollider != null)
            {
                boundaryCollider.enabled = enable;
            }
        }
    }

    private void SpawnEnemy()
    {
        // Спавним врага на заданной точке и сохраняем ссылку на него
        spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}