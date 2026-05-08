using UnityEngine;

public class ArcherAttack : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float projectileSpeed = 4f;
    [SerializeField] private float damage = 10f;
    private float lastAttackTime;

    public void Attack(Transform self, Transform target)
    {
        float distance = Vector2.Distance(self.position, target.position);
        if (distance > attackRange) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        SpawnProjectile(self, target);
    }

    private void SpawnProjectile(Transform self, Transform target)
    {
        Vector2 direction = (target.position - self.position).normalized;

        GameObject projectile = new GameObject("EnemyProjectile");
        projectile.transform.position = self.position;

        EnemyProjectile ep = projectile.AddComponent<EnemyProjectile>();
        ep.Initialize(direction, projectileSpeed, damage);
    }
}