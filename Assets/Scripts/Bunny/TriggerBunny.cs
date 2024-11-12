using UnityEngine;
using System.Collections;

public class TriggerBunny : MonoBehaviour
{
    public Transform player; // Игрок
    public Camera mainCamera; // Камера
    private bool isCameraLocked = false; // Флаг фиксации камеры
    private CameraFollow cameraFollowScript; // Ссылка на скрипт CameraFollow
    private bool hasTriggered = false; // Флаг, чтобы действие выполнялось один раз

    private void Start()
    {
        // Получаем ссылку на скрипт CameraFollow на камере
        cameraFollowScript = mainCamera.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если игрок входит в зону и триггер ещё не сработал, фиксируем камеру
        if (other.CompareTag("Player") && !isCameraLocked && !hasTriggered)
        {
            LockCamera();
            EnableBoundaryColliders(true); // Включаем коллайдеры Boundary
            StartCoroutine(EnableBunnyObjectsAfterDelay(1f)); // Включаем объекты Bunny через секунду
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

    private IEnumerator EnableBunnyObjectsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Находим все объекты с тегом "Bunny" и включаем их
        GameObject[] bunnies = GameObject.FindGameObjectsWithTag("Bunny");

        foreach (var bunny in bunnies)
        {
            bunny.SetActive(true);
        }
    }
}