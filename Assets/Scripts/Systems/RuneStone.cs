using UnityEngine;

public class RuneStone : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthRegenPerSecond = 2f; // Adjust this to balance difficulty
    private float currentHealth;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Regenerate health over time (if not at max and not dead)
        if (!IsDead && currentHealth < maxHealth)
        {
            currentHealth += healthRegenPerSecond * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth); // Cap at max
            GameEvents.CoreDamaged(currentHealth); // Update health bar
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore player spells - don't let them damage the RuneStone
        if (other.CompareTag("spell"))
        {
            return;
        }
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        GameEvents.CoreDamaged(currentHealth);

        if (IsDead)
        {
            GameEvents.CoreDestroyed();
            Debug.Log("Rune Stone destroyed! Game Over.");
        }
    }
}