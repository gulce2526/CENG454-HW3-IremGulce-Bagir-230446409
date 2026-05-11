using UnityEngine;
using System.Collections;

public class FrostDecorator : SpellDecorator
{
    [SerializeField] private float bonusDamage = 5f;
    [SerializeField] private float bonusManaCost = 3f;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private ObjectPool projectilePool;

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
            pp.Initialize(direction, GetDamage(), GetRange());
        }
    }

    public override void Apply(GameObject target)
    {
        wrappedSpell.Apply(target);

        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
            StartCoroutine(ApplySlow(enemy));
    }

    private IEnumerator ApplySlow(Enemy enemy)
    {
        enemy.ApplySlow(slowAmount);
        yield return new WaitForSeconds(slowDuration);
        if (enemy != null)
            enemy.RemoveSlow();
    }
}