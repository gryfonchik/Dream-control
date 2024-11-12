using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; // Ссылка на диалог, который будет отображаться

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что это игрок вошел в триггер
        if (other.CompareTag("Player"))
        {
            // Начинаем диалог
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}

