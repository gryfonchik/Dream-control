using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneTrigger : MonoBehaviour
{
    public Transform player; // Игрок
    public Camera mainCamera; // Камера
    public GameObject enemyPrefab; // Префаб врага (червяка)
    public Transform[] spawnPoints; // Массив точек для спавна врагов
    public float spawnInterval = 2f; // Интервал спавна врагов в секундах
    private bool isCameraLocked = false; // Флаг фиксации камеры
    private CameraFollow cameraFollowScript; // Ссылка на скрипт CameraFollow
    private bool hasTriggered = false; // Флаг, чтобы действие выполнялось один раз
    private Coroutine spawnCoroutine; // Ссылка на корутину спавна
    private bool hasSpawned = false; // Флаг, что враги уже заспавнены

    private int[] spawnCounts; // Массив для отслеживания числа спавнов из каждой точки
    private List<int> availableSpawnIndices; // Список доступных индексов для спавна

    private void Start()
    {
        // Получаем ссылку на скрипт CameraFollow на камере
        cameraFollowScript = mainCamera.GetComponent<CameraFollow>();
        spawnCounts = new int[spawnPoints.Length]; // Инициализируем массив для отслеживания числа спавнов
        availableSpawnIndices = new List<int>(); // Инициализируем список доступных спавнпоинтов

        // Добавляем все индексы спавнпоинтов в список доступных
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableSpawnIndices.Add(i);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если игрок входит в зону и триггер ещё не сработал, фиксируем камеру и запускаем спавн врагов
        if (other.CompareTag("Player") && !isCameraLocked && !hasTriggered)
        {
            LockCamera();
            StartSpawningEnemies();
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

        // Останавливаем спавн врагов
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
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

    private void StartSpawningEnemies()
    {
        // Запускаем корутину для спавна врагов
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (availableSpawnIndices.Count > 0) // Продолжаем, пока есть доступные спавнпоинты
        {
            // Ждем интервал перед спавном
            yield return new WaitForSeconds(spawnInterval);

            // Выбираем случайный доступный индекс спавнпоинта
            int randomIndex = availableSpawnIndices[Random.Range(0, availableSpawnIndices.Count)];

            // Создаем врага на выбранной точке
            Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            spawnCounts[randomIndex]++; // Увеличиваем счетчик спавнов для данной точки
            hasSpawned = true; // Устанавливаем, что враги заспавнены

            // Если из текущего спавнпоинта заспавнилось два червя, убираем его из списка доступных
            if (spawnCounts[randomIndex] >= 2)
            {
                availableSpawnIndices.Remove(randomIndex);
            }
        }

        // После завершения спавна следим за червями на сцене
        StartCoroutine(CheckEnemiesAndUnlockCamera());
    }

    private IEnumerator CheckEnemiesAndUnlockCamera()
    {
        // Ждем и проверяем, остались ли черви
        while (GameObject.FindGameObjectsWithTag("Worm").Length > 0)
        {
            yield return new WaitForSeconds(1f);
        }

        // Если червей нет, разблокируем камеру
        UnlockCamera();
    }
}