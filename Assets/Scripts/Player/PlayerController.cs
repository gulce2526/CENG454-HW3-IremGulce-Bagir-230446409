using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        animator.SetBool("isRun", moveInput.magnitude > 0);

        if (moveInput.x != 0)
            transform.localScale = new Vector3(
                moveInput.x > 0 ? 0.35f : -0.35f, 0.35f, 1);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        currentHealth -= amount;
        if (IsDead) Debug.Log("Sorcerer defeated!");
    }
}