using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IPoolable
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifeTime = 5f;
    private float spawnTime;

    public void Initialize(Vector2 dir, float spd, float dmg)
    {
        direction = dir;
        speed = spd;
        damage = dmg;
        spawnTime = Time.time;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (Time.time - spawnTime > lifeTime)
            OnReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            OnReturnToPool();
        }
    }

    public void OnSpawn()
    {
        spawnTime = Time.time;
    }

    public void OnReturnToPool()
    {
        Destroy(gameObject);
    }
}