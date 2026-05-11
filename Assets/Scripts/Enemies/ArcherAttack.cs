using UnityEngine;

public class ArcherAttack : MonoBehaviour, IAttackStrategy
{
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float projectileSpeed = 5f;
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
        if (EnemyProjectilePool.Instance == null) return;
        ObjectPool pool = EnemyProjectilePool.Instance.GetPool();
        if (pool == null) return;

        Vector2 direction = (target.position - self.position).normalized;

        GameObject projectile = pool.Get();
        projectile.transform.position = self.position;

        EnemyProjectile ep = projectile.GetComponent<EnemyProjectile>();
        if (ep != null)
        {
            ep.SetPool(pool);
            ep.Initialize(direction, projectileSpeed, damage);
        }
    }
}