using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Получаем компонент Animator объекта
        animator = GetComponent<Animator>();
    }

    // Этот метод будет вызываться через Animation Event
    public void DestroyObject()
    {
        // Удаляем объект
        Destroy(gameObject);
    }
}

