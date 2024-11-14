using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    // Задаем позицию для респавна
    public Transform respawnPoint;  // Точка, куда будет перемещаться игрок при падении

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, игрок ли попал в коллайдер
        if (other.CompareTag("Player"))
        {
            // Перемещаем игрока на точку респавна
            other.transform.position = respawnPoint.position;
            
            // Обнуляем скорость (если есть Rigidbody) для предотвращения сохранения импульса падения
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}

