using UnityEngine;

public class PlayerProjectile : MonoBehaviour, IPoolable
{
    private float damage;
    private float range;
    private Vector2 startPosition;
    private Vector2 direction;
    [SerializeField] private float speed = 8f;

    public void Initialize(Vector2 dir, float dmg, float rng)
    {
        direction = dir;
        damage = dmg;
        range = rng;
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        float distanceTravelled = Vector2.Distance(startPosition, transform.position);
        if (distanceTravelled >= range)
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
        startPosition = transform.position;
    }

    public void OnReturnToPool()
    {
        Destroy(gameObject);
    }
}