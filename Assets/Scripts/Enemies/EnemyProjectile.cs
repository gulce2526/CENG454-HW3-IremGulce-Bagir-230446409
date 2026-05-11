using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IPoolable
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private float lifeTime = 5f;
    private float spawnTime;
    private ObjectPool myPool;

    public void Initialize(Vector2 dir, float spd, float dmg)
    {
        direction = dir;
        speed = spd;
        damage = dmg;
        spawnTime = Time.time;
    }

    public void SetPool(ObjectPool pool)
    {
        myPool = pool;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (Time.time - spawnTime > lifeTime)
            ReturnSelf();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;  

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            ReturnSelf();
        }
    }

    private void ReturnSelf()
    {
        if (myPool != null)
            myPool.Return(gameObject);
        else
            gameObject.SetActive(false);
    }

    public void OnSpawn()
    {
        spawnTime = Time.time;
    }

    public void OnReturnToPool()
    {
        direction = Vector2.zero;
        speed = 0;
        damage = 0;
    }
}