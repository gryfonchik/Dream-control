using UnityEngine;

// Базовый класс
public class CharacterBase : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    protected Collider2D collider2d;
    protected float movementSpeed;

    // Метод Awake для автоматической инициализации
    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();

        // Проверка наличия компонентов
        if (rb2d == null)
        {
            Debug.LogError($"{name} требует компонент Rigidbody2D!");
        }
        if (collider2d == null)
        {
            Debug.LogError($"{name} требует компонент Collider2D!");
        }
    }

    // Метод для установки скорости движения
    public virtual void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }
}

