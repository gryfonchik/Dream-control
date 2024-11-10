// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     [SerializeField] private Transform player; // Персонаж, за которым следует камера
//     [SerializeField] private float smoothTime = 0.3f; // Время сглаживания
//     [SerializeField] private float leftLimit; // Левая граница уровня
//     [SerializeField] private float rightLimit; // Правая граница уровня
//     [SerializeField] private float followThreshold = 2.0f; // Порог следования камеры
//     [SerializeField] private float offsetDistance = 1.0f; // Смещение зоны относительно игрока

//     private Vector3 initialOffset; // Начальное смещение камеры
//     private float previousPlayerX; // Предыдущее положение игрока по оси X
//     private Vector3 velocity = Vector3.zero; // Скорость для SmoothDamp

//     private void Start()
//     {
//         // Сохраняем начальное смещение от игрока
//         initialOffset = transform.position - player.position;
//         previousPlayerX = player.position.x;
//     }

//     private void LateUpdate()
//     {
//         // Текущая позиция игрока и камеры
//         float playerX = player.position.x;
//         float cameraX = transform.position.x;

//         // Определяем направление движения игрока
//         float direction = Mathf.Sign(playerX - previousPlayerX);

//         // Смещаем зону в зависимости от направления
//         float thresholdPosition = playerX + direction * offsetDistance;

//         // Проверяем, когда игрок выходит за пределы зоны следования
//         if ((direction > 0 && cameraX < thresholdPosition - followThreshold) ||
//             (direction < 0 && cameraX > thresholdPosition + followThreshold))
//         {
//             // Двигаем камеру за игроком с учетом начального смещения
//             Vector3 targetPosition = new Vector3(thresholdPosition, player.position.y + initialOffset.y, transform.position.z);
            
//             // Плавное сглаживание движения камеры с использованием SmoothDamp
//             Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

//             // Ограничиваем движение камеры по границам уровня
//             smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, leftLimit, rightLimit);

//             // Применяем обновленную позицию
//             transform.position = smoothedPosition;
//         }

//         // Обновляем предыдущее положение игрока
//         previousPlayerX = playerX;
//     }

//     private void OnDrawGizmos()
//     {
//         // Отображение границ уровня в редакторе Unity
//         Gizmos.color = Color.red;
//         Gizmos.DrawLine(new Vector3(leftLimit, transform.position.y - 5, 0), new Vector3(leftLimit, transform.position.y + 5, 0));
//         Gizmos.DrawLine(new Vector3(rightLimit, transform.position.y - 5, 0), new Vector3(rightLimit, transform.position.y + 5, 0));
//     }
// }

using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float damping = 1.5f;
    public Vector2 offset = new Vector2(2f, 1f);
    public bool faceLeft;
    private Transform player;
    private int lastX;

    void Start ()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer(faceLeft);
    }

    public void FindPlayer(bool playerFaceLeft)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastX = Mathf.RoundToInt(player.position.x);
        if(playerFaceLeft)
        {
            transform.position = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
    }

    void LateUpdate () 
    {
        if(player)
        {
            int currentX = Mathf.RoundToInt(player.position.x);
            if(currentX > lastX) faceLeft = false; else if(currentX < lastX) faceLeft = true;
            lastX = Mathf.RoundToInt(player.position.x);

            Vector3 target;
            if(faceLeft)
            {
                target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
            }
            else
            {
                target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            }
            Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
            transform.position = currentPosition;
        }
    }
}
