using UnityEngine;
public class Boundary : MonoBehaviour
{
    public BoxCollider2D leftBoundary; // Левый коллайдер
    public BoxCollider2D rightBoundary; // Правый коллайдер
    public Camera mainCamera; // Ссылка на камеру
    public CameraFollow cameraFollowScript; // Скрипт, отвечающий за слежение камеры за игроком

    private bool isCameraLocked = false;
    private float timer = 0f;
    public float lockTime = 5f; // Время блокировки камеры

    void Start()
    {
        // Отключаем боковые коллайдеры на старте
        leftBoundary.enabled = false;
        rightBoundary.enabled = false;
    }

    void Update()
    {
        if (isCameraLocked)
        {
            timer += Time.deltaTime;
            if (timer >= lockTime)
            {
                UnlockCamera();
            }
        }
    }

    private void LockCamera()
    {
        isCameraLocked = true;
        timer = 0f;

        // Отключаем слежение и позиционируем камеру
        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = false;
        }

        // Получаем ширину камеры и позиционируем её
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        Vector3 newCameraPosition = mainCamera.transform.position;
        newCameraPosition.x = transform.position.x + cameraHalfWidth; // Меняем только X координату
        mainCamera.transform.position = newCameraPosition;

        // Активируем боковые коллайдеры
        leftBoundary.enabled = true;
        rightBoundary.enabled = true;
    }

    private void UnlockCamera()
    {
        isCameraLocked = false;

        // Включаем слежение за игроком
        if (cameraFollowScript != null)
        {
            cameraFollowScript.enabled = true;
        }

        // Деактивируем боковые коллайдеры
        leftBoundary.enabled = false;
        rightBoundary.enabled = false;
    }
}