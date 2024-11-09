using UnityEngine;

public class EnemySpawnerOnTrigger : MonoBehaviour
{
    public GameObject enemyPrefab;      // Префаб врага, который появится
    public Transform[] spawnPoints;     // Точки появления врагов
    private bool hasSpawned = false;    // Флаг, чтобы предотвратить повторное появление врагов

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверка, является ли объект игроком и что враги ещё не были созданы
        if (other.CompareTag("Player") && !hasSpawned)
        {
            SpawnEnemies();
            hasSpawned = true; // Запрещаем повторное появление врагов
        }
    }

    private void SpawnEnemies()
    {

        foreach (Transform spawnPoint in spawnPoints)
        {
            // Создаём врага на каждой точке из массива spawnPoints
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Получаем компонент Animator у созданного врага
            Animator enemyAnimator = enemy.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                // Устанавливаем триггер, чтобы запустить анимацию
                enemyAnimator.SetTrigger("Trigger_Connect");
            }
            else
            {
                Debug.LogWarning("Animator не найден на префабе врага!");
            }

            // Передаем позицию триггера в скрипт движения врага
            Worm_Move enemyMovement = enemy.GetComponent<Worm_Move>();
            if (enemyMovement != null)
            {
                // Передаем позицию только по оси X
                Vector3 triggerPosition = new Vector3(transform.position.x, enemyMovement.transform.position.y, enemyMovement.transform.position.z);
                enemyMovement.target = transform; // Устанавливаем текущий объект (триггер) как цель для врага
            }
            else
            {
                Debug.LogWarning("EnemyMovement не найден на префабе врага!");
            }
        }
    }
}









