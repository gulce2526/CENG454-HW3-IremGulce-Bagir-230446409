using UnityEngine;

public class LoveDecorator : SpellDecorator
{
    [SerializeField] private float bonusDamage = 5f;
    [SerializeField] private float bonusManaCost = 4f;
    [SerializeField] private ObjectPool projectilePool;
    [SerializeField] private GameObject loveHitVFX;

    public override float GetDamage() => wrappedSpell.GetDamage() + bonusDamage;
    public override float GetManaCost() => wrappedSpell.GetManaCost() + bonusManaCost;

    public override void Cast(Vector2 direction, Vector2 origin)
    {
        if (projectilePool == null)
        {
            wrappedSpell.Cast(direction, origin);
            return;
        }

        GameObject proj = projectilePool.Get();
        proj.transform.position = origin;
        proj.transform.rotation = Quaternion.identity;

        PlayerProjectile pp = proj.GetComponent<PlayerProjectile>();
        if (pp != null)
        {
            pp.SetPool(projectilePool);
            pp.SetSourceSpell(this);
            pp.SetHitVFX(loveHitVFX);
            pp.Initialize(direction, GetDamage(), GetRange());
        }
    }

    public override void Apply(GameObject target)
    {
        wrappedSpell.Apply(target);
    }
}