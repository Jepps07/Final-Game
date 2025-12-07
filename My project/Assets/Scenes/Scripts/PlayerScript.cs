using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 9f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint; // create empty child of Player at gun muzzle
    public float bulletSpeed = 12f;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded = false;
    private float horizontal = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        HandleInput();
        HandleAnimations();
        if (Input.GetKeyDown(KeyCode.Mouse0)) // left click or K
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        // Left/Right movement with A/D or LeftArrow/RightArrow
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
        horizontal = moveX;
    }

    void Move()
    {
        // Side-scroller: only horizontal velocity, vertical controlled by physics (jump)
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        // Flip sprite
        if (horizontal > 0.01f) transform.localScale = new Vector3(1, 1, 1);
        else if (horizontal < -0.01f) transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleAnimations()
    {
        // Walking
        anim.SetFloat("Speed", Mathf.Abs(horizontal));

        // Crouch (S or LeftControl)
        bool crouch = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftControl);
        anim.SetBool("IsCrouching", crouch);

        // Jump press
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            anim.SetBool("IsJumping", true);
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        // set direction based on facing
        float dir = transform.localScale.x > 0 ? 1f : -1f;
        b.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(dir * bulletSpeed, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground detection
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("IsJumping", false);
        }

        // Boss projectile hits player
        if (collision.collider.CompareTag("BossProjectile") || collision.collider.CompareTag("Boss"))
        {
            // If boss projectile uses trigger, see OnTriggerEnter2D instead.
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Many projectiles are set as triggers
        if (other.CompareTag("BossProjectile"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        // play hurt animation if you have one (optional)
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        anim.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
        this.enabled = false; // stops input/movement
        // Optionally: StartCoroutine(Respawn()) or reload scene
    }
}