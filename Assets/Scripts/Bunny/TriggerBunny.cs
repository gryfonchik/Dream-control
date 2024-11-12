using UnityEngine;
using System.Collections;

public class TriggerBunny : MonoBehaviour
{
    public Transform player; // Игрок
    public Camera mainCamera; // Камера
    public GameObject[] bunnyObjects; // Список объектов, которые нужно включить (например, объекты Bunny)
    public GameObject commonBunny; // Префаб "заяц общий" уже на сцене
    private bool isCameraLocked = false; // Флаг фиксации камеры
    private CameraFollow cameraFollowScript; // Ссылка на скрипт CameraFollow
    private bool hasTriggered = false; // Флаг, чтобы действие выполнялось один раз
    private BoxCollider2D zoneCollider; // Коллайдер триггера

    private void Start()
    {
        // Получаем ссылку на скрипт CameraFollow на камере
        cameraFollowScript = mainCamera.GetComponent<CameraFollow>();

        // Получаем BoxCollider2D триггера
        zoneCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если игрок входит в зону и триггер ещё не сработал, фиксируем камеру
        if (other.CompareTag("Player") && !isCameraLocked && !hasTriggered)
        {
            LockCamera();
            EnableBoundaryColliders(true); // Включаем коллайдеры Boundary
            StartCoroutine(EnableObjectsAfterDelay(1f)); // Включаем объекты через секунду
            hasTriggered = true; // Устанавливаем, что триггер сработал
            StartCoroutine(CheckCommonBunnyAndUnlockCamera()); // Запускаем проверку удаления "заяц общий"
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

        // Располагаем камеру по левому нижнему углу коллайдера триггера
        Vector3 newCameraPosition = mainCamera.transform.position;
        newCameraPosition.x = zoneCollider.bounds.min.x + mainCamera.orthographicSize * mainCamera.aspect;
        newCameraPosition.y = zoneCollider.bounds.min.y + mainCamera.orthographicSize;
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

    private IEnumerator EnableObjectsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Включаем все объекты из массива bunnyObjects
        foreach (var bunny in bunnyObjects)
        {
            if (bunny != null)
            {
                bunny.SetActive(true);
            }
        }

        // Включаем "заяц общий", если он не включен
        if (commonBunny != null)
        {
            commonBunny.SetActive(true);
        }
    }

    private IEnumerator CheckCommonBunnyAndUnlockCamera()
    {
        // Проверяем, пока объект "заяц общий" существует
        while (commonBunny != null)
        {
            yield return new WaitForSeconds(1f); // Проверяем каждую секунду
        }

        // Когда "заяц общий" удалён, отключаем коллайдеры и разблокируем камеру
        EnableBoundaryColliders(false);
        UnlockCamera();
    }

    private void UnlockCamera()
    {
        isCameraLocked = false;

        // Включаем слежение за игроком
        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = true;
        }
    }
}