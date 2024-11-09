using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timeDestroy = 3f;
    public float speed = 20f;
    public Rigidbody2D rb;

    private Vector3 previousPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diference.z = 0;
        diference.Normalize();

        float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);

        rb.velocity = diference * speed;

        previousPosition = transform.position;

        Invoke("DestroyBullet", timeDestroy);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        // Создаем Raycast от предыдущей позиции к текущей позиции
        RaycastHit2D hit = Physics2D.Raycast(previousPosition, transform.position - previousPosition, Vector2.Distance(previousPosition, transform.position));

        // Если Raycast обнаружил объект с тегом Enemy
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Destroy(hit.collider.gameObject); // Уничтожаем противника
            Destroy(gameObject); // Уничтожаем пулю
        }

        previousPosition = transform.position; // Обновляем предыдущую позицию
    }
}



