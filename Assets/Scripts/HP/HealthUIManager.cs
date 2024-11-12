using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public PlayerHealth playerHealth; // Ссылка на скрипт PlayerHealth
    public TextMeshProUGUI healthText; // Ссылка на UI-текст для здоровья

    void Update()
    {
        // Проверяем, что ссылки установлены
        if (playerHealth != null && healthText != null)
        {
            // Обновляем текст UI в зависимости от текущего здоровья
            healthText.text = $"HP: {playerHealth.GetCurrentHealth()}";
        }
    }
}