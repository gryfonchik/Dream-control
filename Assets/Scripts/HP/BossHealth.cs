using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10; // Максимальное здоровье противника
    private int currentHealth; // Текущее здоровье противника

    private void Start()
    {
        // Инициализация текущего здоровья противника при старте
        currentHealth = maxHealth;
    }

    // Метод для нанесения урона противнику
    public void TakeDamage(int damageAmount)
    {
        // Уменьшаем здоровье на количество урона
        currentHealth -= damageAmount;

        // Если здоровье опустилось ниже или равно нулю, вызываем метод смерти
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


    // Метод для получения текущего здоровья
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}

