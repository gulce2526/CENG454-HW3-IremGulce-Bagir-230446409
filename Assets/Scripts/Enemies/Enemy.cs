using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IPoolable
{
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private float playerDetectionRange = 6f;
    [SerializeField] private bool ignoresPlayer = false;

    private float currentHealth;
    private IMovementStrategy movementStrategy;
    private IAttackStrategy attackStrategy;

    private Transform playerTarget;
    private Transform coreTarget;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    public void SetStrategies(IMovementStrategy movement, IAttackStrategy attack)
    {
        movementStrategy = movement;
        attackStrategy = attack;
    }

    public void SetTargets(Transform player, Transform core)
    {
        playerTarget = player;
        coreTarget = core;
    }

    private Transform GetCurrentTarget()
    {
        if (ignoresPlayer || playerTarget == null) return coreTarget;

        float distanceToPlayer = Vector2.Distance(
            transform.position, playerTarget.position);

        return distanceToPlayer <= playerDetectionRange
            ? playerTarget
            : coreTarget;
    }

    private void Update()
    {
        if (IsDead) return;
        Transform currentTarget = GetCurrentTarget();
        if (currentTarget == null) return;

        movementStrategy?.Move(transform, currentTarget);
        attackStrategy?.Attack(transform, currentTarget);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        currentHealth -= amount;
        if (IsDead) Die();
    }

    private void Die()
    {
        GameEvents.EnemyKilled(scoreValue);
        OnReturnToPool();
    }

    public void OnSpawn()
    {
        currentHealth = maxHealth;
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }
}