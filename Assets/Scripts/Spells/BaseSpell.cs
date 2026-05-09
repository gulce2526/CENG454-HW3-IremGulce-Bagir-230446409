using UnityEngine;

public class BaseSpell : MonoBehaviour, ISpellEffect
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float manaCost = 5f;
    [SerializeField] private float range = 5f;
    [SerializeField] private GameObject projectilePrefab;

    public virtual float GetDamage() => damage;
    public virtual float GetManaCost() => manaCost;
    public virtual float GetRange() => range;

    public virtual void Cast(Vector2 direction, Vector2 origin)
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, origin, Quaternion.identity);
        PlayerProjectile pp = proj.GetComponent<PlayerProjectile>();
        if (pp != null)
            pp.Initialize(direction, GetDamage(), GetRange());
    }

    public virtual void Apply(GameObject target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
            damageable.TakeDamage(GetDamage());
    }
}