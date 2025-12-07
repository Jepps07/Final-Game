using UnityEngine;

public class BossScript : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public Rigidbody2D rb;

    [Header("Shooting")]
    public GameObject bossProjectilePrefab;
    public Transform shootPoint;
    public float shootInterval = 1.2f;
    public float shootDelay = 1f;

    void Start()
    {
        currentHealth = maxHealth;
        if (bossProjectilePrefab != null && shootPoint != null)
            InvokeRepeating(nameof(Shoot), shootDelay, shootInterval);
        rb = GetComponent<Rigidbody2D>();
    }
    void Shoot()
    {
        GameObject p = Instantiate(bossProjectilePrefab, shootPoint.position, Quaternion.identity);
        // choose direction toward player (optional): send left by default
        // You can set velocity in BossProjectile script using its own logic.
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // play boss death VFX / animation
        Destroy(gameObject);
    }
}
