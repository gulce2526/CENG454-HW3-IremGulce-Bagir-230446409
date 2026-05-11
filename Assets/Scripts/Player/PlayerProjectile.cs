using UnityEngine;

public class PlayerProjectile : MonoBehaviour, IPoolable
{
    private float damage;
    private float range;
    private Vector2 startPosition;
    private Vector2 direction;
    [SerializeField] private float speed = 8f;
    private ObjectPool myPool;
    private ISpellEffect sourceSpell;
    private GameObject hitVFXPrefab;

    public void Initialize(Vector2 dir, float dmg, float rng)
    {
        direction = dir.normalized;
        damage = dmg;
        range = rng;
        startPosition = transform.position;
    }

    public void SetPool(ObjectPool pool) => myPool = pool;
    public void SetSourceSpell(ISpellEffect spell) => sourceSpell = spell;
    public void SetHitVFX(GameObject vfx) => hitVFXPrefab = vfx;

    private void Update()
    {
        if (direction == Vector2.zero) return;

        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        float distanceTravelled = Vector2.Distance(startPosition, transform.position);
        if (distanceTravelled >= range)
            ReturnSelf();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("RuneStone")) return; // Don't damage our own RuneStone!

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            if (sourceSpell != null)
                sourceSpell.Apply(other.gameObject);
            else
                target.TakeDamage(damage);

            if (hitVFXPrefab != null)
                Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);

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

    public void OnSpawn() => startPosition = transform.position;

    public void OnReturnToPool()
    {
        direction = Vector2.zero;
        damage = 0;
        sourceSpell = null;
        hitVFXPrefab = null;
    }
}