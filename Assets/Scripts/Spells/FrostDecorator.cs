using UnityEngine;
using System.Collections;

public class FrostDecorator : SpellDecorator
{
    [SerializeField] private float bonusDamage = 5f;
    [SerializeField] private float bonusManaCost = 3f;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private GameObject frostProjectilePrefab;

    public override float GetDamage() => wrappedSpell.GetDamage() + bonusDamage;
    public override float GetManaCost() => wrappedSpell.GetManaCost() + bonusManaCost;

    public override void Cast(Vector2 direction, Vector2 origin)
    {
        if (frostProjectilePrefab != null)
        {
            GameObject proj = Instantiate(frostProjectilePrefab, origin, Quaternion.identity);
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
        StartCoroutine(ApplySlow(target));
    }

    private IEnumerator ApplySlow(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy == null) yield break;

        enemy.ApplySlow(slowAmount);
        yield return new WaitForSeconds(slowDuration);
        enemy.RemoveSlow();
    }
}