using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IPoolable
{
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private float playerDetectionRange = 6f;
    [SerializeField] private bool ignoresPlayer = false;
    [SerializeField] private GemType gemType = GemType.Green;

    private float currentHealth;
    private IMovementStrategy movementStrategy;
    private IAttackStrategy attackStrategy;

    private Transform playerTarget;
    private Transform coreTarget;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    private void Start()
    {
        movementStrategy = GetComponent<IMovementStrategy>();
        attackStrategy = GetComponent<IAttackStrategy>();
        currentHealth = maxHealth;
    }

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

    public void ApplySlow(float multiplier)
    {
        RushMovement rush = GetComponent<RushMovement>();
        if (rush != null) rush.SetSpeedMultiplier(multiplier);

        ArcherMovement archer = GetComponent<ArcherMovement>();
        if (archer != null) archer.SetSpeedMultiplier(multiplier);

        BruteMovement brute = GetComponent<BruteMovement>();
        if (brute != null) brute.SetSpeedMultiplier(multiplier);
    }

    public void RemoveSlow() => ApplySlow(1f);

    private void Die()
    {
        ObjectPool pool = GemPoolManager.Instance != null
            ? GemPoolManager.Instance.GetPool(gemType)
            : null;

        if (pool != null)
        {
            GameObject orb = pool.Get();
            orb.transform.position = transform.position;
            OrbPickup pickup = orb.GetComponent<OrbPickup>();
            if (pickup != null)
                pickup.SetPool(pool);
        }

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