using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    public void Attack(Transform self, Transform target)
    {
        float distance = Vector2.Distance(self.position, target.position);
        if (distance > attackRange) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(damage);
    }
}