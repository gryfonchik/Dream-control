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
    public int enemyCount = 2; // Количество врагов

    private int[] spawnCounts; // Массив для отслеживания числа спавнов из каждой точки
    private List<int> availableSpawnIndices; // Список доступных индексов для спавна

    private void Start()
    {
        cameraFollowScript = mainCamera.GetComponent<CameraFollow>();
        spawnCounts = new int[spawnPoints.Length];
        availableSpawnIndices = new List<int>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableSpawnIndices.Add(i);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCameraLocked && !hasTriggered)
        {
            LockCamera();
            StartSpawningEnemies();
            hasTriggered = true;
        }
    }

    private void LockCamera()
    {
        isCameraLocked = true;

        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = false;
        }

        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        Vector3 newCameraPosition = mainCamera.transform.position;
        newCameraPosition.x = transform.position.x + cameraHalfWidth;
        mainCamera.transform.position = newCameraPosition;

        EnableBoundaryColliders(true);
    }

    private void UnlockCamera()
    {
        isCameraLocked = false;

        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = true;
        }

        EnableBoundaryColliders(false);

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    private void EnableBoundaryColliders(bool enable)
    {
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
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (availableSpawnIndices.Count > 0)
        {
            yield return new WaitForSeconds(spawnInterval);

            int randomIndex = availableSpawnIndices[Random.Range(0, availableSpawnIndices.Count)];
            Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
            spawnCounts[randomIndex]++;
            hasSpawned = true;

            if (spawnCounts[randomIndex] >= enemyCount)
            {
                availableSpawnIndices.Remove(randomIndex);
            }
        }

        StartCoroutine(CheckEnemiesAndUnlockCamera());
    }

    private IEnumerator CheckEnemiesAndUnlockCamera()
    {
        while (GetEnemyCountByName("Worm(Clone)") > 0)
        {
            yield return new WaitForSeconds(1f);
        }

        UnlockCamera();
    }

    private int GetEnemyCountByName(string enemyName)
    {
        int count = 0;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name == enemyName)
            {
                count++;
            }
        }

        return count;
    }
}