using UnityEngine;

public class LoveDecorator : SpellDecorator
{
    [SerializeField] private float bonusDamage = 5f;
    [SerializeField] private float bonusManaCost = 4f;
    [SerializeField] private float aoeRadius = 2f;
    [SerializeField] private GameObject loveProjectilePrefab;
    [SerializeField] private GameObject loveHitVFX;

    public override float GetDamage() => wrappedSpell.GetDamage() + bonusDamage;
    public override float GetManaCost() => wrappedSpell.GetManaCost() + bonusManaCost;

    public override void Cast(Vector2 direction, Vector2 origin)
    {
        if (loveProjectilePrefab != null)
        {
            GameObject proj = Instantiate(loveProjectilePrefab, origin, Quaternion.identity);
            PlayerProjectile pp = proj.GetComponent<PlayerProjectile>();
            if (pp != null)
                pp.Initialize(direction, GetDamage(), GetRange());
        }
        else
            wrappedSpell.Cast(direction, origin);
    }

    public override void Apply(GameObject target)
    {
        wrappedSpell.Apply(target);

        if (loveHitVFX != null)
            Instantiate(loveHitVFX, target.transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            target.transform.position, aoeRadius);

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(GetDamage() * 0.5f);
        }
    }
}