using UnityEngine;

public class KeepObject : MonoBehaviour
{
    void Awake()
    {
        // Делает объект неуничтожаемым при смене сцен
        DontDestroyOnLoad(gameObject);
    }
}

