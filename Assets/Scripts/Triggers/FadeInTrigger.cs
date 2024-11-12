using UnityEngine;
using System.Collections;

public class FadeInSpriteTrigger : MonoBehaviour
{
    public SpriteRenderer targetSprite; // Ссылка на спрайт-объект, который будем проявлять
    public float fadeDuration = 2f;     // Время, за которое объект полностью проявится

    private bool hasTriggered = false;  // Флаг, чтобы триггер срабатывал только один раз

    private void Start()
    {
        // Убедимся, что спрайт-объект не прозрачный по умолчанию
        if (targetSprite != null)
        {
            Color color = targetSprite.color;
            color.a = 0; // Начальная прозрачность 0 (полностью прозрачный)
            targetSprite.color = color;
        }
        else
        {
            Debug.LogWarning("Target SpriteRenderer is not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Если игрок входит в триггер и эффект ещё не запускался
        if (other.CompareTag("Player") && !hasTriggered && targetSprite != null)
        {
            StartCoroutine(FadeIn()); // Запускаем корутину для плавного появления
            hasTriggered = true; // Помечаем, что триггер уже сработал
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f; // Время, прошедшее с начала проявления

        while (elapsedTime < fadeDuration)
        {
            // Увеличиваем значение alpha от 0 до 1 за время fadeDuration
            Color color = targetSprite.color;
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            targetSprite.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Устанавливаем alpha на 1, чтобы объект был полностью видим
        Color finalColor = targetSprite.color;
        finalColor.a = 1;
        targetSprite.color = finalColor;
    }
}