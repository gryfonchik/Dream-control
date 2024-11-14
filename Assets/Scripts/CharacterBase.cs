using UnityEngine;

public class CharacterBase : MonoBehaviour
{

    [Header("Основные параметры физики")]
    public float fallMultiplier = 2.5f; // Множитель гравитации для ускорения падения
    public float lowJumpMultiplier = 2f; // Множитель гравитации для уменьшения высоты прыжка при быстром отпускании кнопки


    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void ApplyGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}


