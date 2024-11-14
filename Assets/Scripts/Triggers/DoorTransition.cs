using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DoorTransition : MonoBehaviour
{
    // Название сцены, на которую нужно перейти при взаимодействии
    public string sceneToLoad;

    // Элемент UI CanvasGroup для плавного затемнения экрана
    public CanvasGroup fadeCanvasGroup;

    // Время, за которое экран полностью затемняется
    public float fadeDuration = 1f;

    // Флаг для проверки, можно ли сейчас выполнить переход
    private bool canTransition = false;

    private void Update()
    {
        // Проверяем нажатие клавиши 'E' и наличие возможности перехода
        if (canTransition && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TransitionToScene());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли объект игроком
        if (other.CompareTag("Player"))
        {
            canTransition = true;
            // Можно показать подсказку "Нажмите E для перехода" (например, текст на UI)
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Сбрасываем флаг, если игрок выходит из зоны взаимодействия
        if (other.CompareTag("Player"))
        {
            canTransition = false;
            // Убираем подсказку, если есть
        }
    }

    private IEnumerator TransitionToScene()
    {
        // Начинаем затемнение экрана
        yield return StartCoroutine(FadeOut());

        // Загружаем новую сцену асинхронно
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Ожидаем полной загрузки сцены
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Теперь объект с DontDestroyOnLoad() должен оставаться
        yield return StartCoroutine(FadeIn());
    }


    private IEnumerator FadeOut()
    {
        float fadeSpeed = 1f / fadeDuration;
        float progress = 0f;

        // Постепенно увеличиваем альфа-канал CanvasGroup до 1
        while (progress < 1f)
        {
            progress += Time.deltaTime * fadeSpeed;
            fadeCanvasGroup.alpha = Mathf.Clamp01(progress);
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float fadeSpeed = 1f / fadeDuration;
        float progress = 1f;

        // Постепенно уменьшаем альфа-канал CanvasGroup до 0
        while (progress > 0f)
        {
            progress -= Time.deltaTime * fadeSpeed;
            fadeCanvasGroup.alpha = Mathf.Clamp01(progress);
            yield return null;
        }
    }
}


