using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        // optionally set direction on spawn
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // default shoots left; you can modify for player-targeted shots
            rb.linearVelocity = Vector2.left * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Damage is handled in Player OnTriggerEnter2D
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}