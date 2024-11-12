using System.Collections;
using System.Collections.Generic; // Обязательно добавьте это пространство имен
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueUI; // UI панель диалога
    public TextMeshProUGUI dialogueText;     // Текстовая часть диалога
    public Button continueButton; // Кнопка "Продолжить"

    private Queue<string> sentences; // Очередь для реплик

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        sentences = new Queue<string>();

        // Добавляем слушатель на кнопку "Продолжить"
        continueButton.onClick.AddListener(DisplayNextSentence);
    }

    // Метод для начала диалога
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueUI.SetActive(true); // Показываем окно диалога
        Time.timeScale = 0f;        // Останавливаем время

        sentences.Clear();          // Очищаем предыдущие реплики

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence); // Заполняем очередь новыми репликами
        }

        DisplayNextSentence(); // Показываем первую реплику
    }

    // Метод для отображения следующей реплики
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue(); // Если реплик больше нет, заканчиваем диалог
            return;
        }

        string sentence = sentences.Dequeue(); // Извлекаем реплику из очереди
        dialogueText.text = sentence;          // Отображаем реплику в UI
    }

    // Метод для завершения диалога
    private void EndDialogue()
    {
        dialogueUI.SetActive(false); // Скрываем окно диалога
        Time.timeScale = 1f;         // Возобновляем время
    }
}


