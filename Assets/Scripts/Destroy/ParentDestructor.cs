using UnityEngine;

public class ParentDestructor : MonoBehaviour
{
    public GameObject[] childObjects; // Массив для хранения дочерних объектов, которые отслеживаем

    void Start()
    {
        // Если дочерние объекты не указаны вручную, находим все дочерние объекты
        if (childObjects.Length == 0)
        {
            // Получаем все дочерние объекты
            childObjects = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                childObjects[i] = transform.GetChild(i).gameObject;
            }
        }
    }

    void FixedUpdate()
    {
        // Проверяем каждый дочерний объект, если он уничтожен, удаляем родительский объект
        foreach (var child in childObjects)
        {
            if (child == null) // Если дочерний объект уничтожен
            {
                Destroy(gameObject); // Уничтожаем родительский объект
                return;
            }
        }
    }
}