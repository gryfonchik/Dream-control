using UnityEngine;

public class RandomInputSetup : MonoBehaviour
{
    public KeyCode jumpKey;
    public KeyCode dashKey;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;

    private KeyCode[] availableKeys = new KeyCode[] {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, 
        KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.Space, KeyCode.LeftShift, KeyCode.Tab, 
        KeyCode.LeftControl, KeyCode.LeftAlt, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, 
        KeyCode.Alpha4, KeyCode.Alpha5
    };

    void Start()
    {
        // Случайно выбираем кнопки
        jumpKey = GetRandomKey();
        dashKey = GetRandomKey();
        moveLeftKey = GetRandomKey();
        moveRightKey = GetRandomKey();

        // Убедимся, что кнопки не совпадают
        while (moveLeftKey == moveRightKey || jumpKey == dashKey || jumpKey == moveLeftKey || jumpKey == moveRightKey)
        {
            moveLeftKey = GetRandomKey();
            moveRightKey = GetRandomKey();
        }

        // Выводим назначенные клавиши в консоль
        Debug.Log("Jump key: " + jumpKey);
        Debug.Log("Dash key: " + dashKey);
        Debug.Log("Move Left key: " + moveLeftKey);
        Debug.Log("Move Right key: " + moveRightKey);
    }

    // Метод для получения случайной кнопки
    KeyCode GetRandomKey()
    {
        return availableKeys[Random.Range(0, availableKeys.Length)];
    }
}


