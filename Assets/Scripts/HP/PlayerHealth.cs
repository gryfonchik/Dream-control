using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Настройка максимального количества HP
    public int maxHealth = 3;
    private int currentHealth;

    // Настройка эффекта при потере HP
    public float damagePerHit = 1f;

    // Настройка времени между потерей HP
    public float damageCooldown = 1f; // Время между потерей HP от одного и того же врага
    private float lastDamageTime = 0f; // Время последнего урона

    // Для отслеживания столкновений с врагами
    private GameObject lastDamagingEnemy = null;

    void Start()
    {
        // Инициализация текущего HP
        currentHealth = maxHealth;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Проверка на столкновение с врагом
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Если с этим врагом еще не получали урон или прошло достаточное время с последнего урона
            if (lastDamagingEnemy != collision.gameObject || Time.time - lastDamageTime >= damageCooldown)
            {
                // Уменьшаем HP при столкновении с врагом
                TakeDamage(damagePerHit, collision.gameObject);
            }
        }
    }

    void TakeDamage(float damage, GameObject enemy)
    {
        // Логируем потерю HP
        Debug.Log($"Игрок получил урон: {damage}. Текущее HP: {currentHealth}");
        
        currentHealth -= (int)damage;

        // Обновляем время последнего урона
        lastDamageTime = Time.time;
        lastDamagingEnemy = enemy;

        // Логируем новое количество HP после урона
        Debug.Log($"Текущее HP после урона: {currentHealth}");

        // Проверяем, если HP меньше или равно 0, перезагружаем сцену
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            ReloadScene();
        }
    }

    void ReloadScene()
    {
        // Получаем текущую сцену и перезагружаем её
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Метод для получения текущего HP (можно использовать для UI)
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}





