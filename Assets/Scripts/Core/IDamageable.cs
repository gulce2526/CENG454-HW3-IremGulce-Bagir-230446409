public interface IDamageable
{
    float MaxHealth { get; }
    float CurrentHealth { get; }
    void TakeDamage(float amount);
    bool IsDead { get; }
}