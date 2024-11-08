using UnityEngine;

// Класс-наследник для управления движением
public class Movement : CharacterBase
{
    // Уникальная скорость, устанавливаемая по умолчанию для игрока
    public float defaultMovementSpeed = 7.0f;

    protected override void Awake()
    {
        base.Awake(); // Вызов базового метода Awake
        movementSpeed = defaultMovementSpeed; // Применение уникальной скорости для игрока
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(move * movementSpeed, rb2d.velocity.y);
    }
}

