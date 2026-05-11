using UnityEngine;

public class RuneStone : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    private void Start()
    {
        currentHealth = maxHealth;
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