using UnityEngine;
using System.Collections;

public class BurnDecorator : SpellDecorator
{
    [SerializeField] private float bonusDamage = 5f;
    [SerializeField] private float bonusManaCost = 3f;
    [SerializeField] private float burnDamage = 2f;
    [SerializeField] private float burnDuration = 3f;
    [SerializeField] private float burnTickRate = 1f;
    [SerializeField] private GameObject burnProjectilePrefab;
    [SerializeField] private GameObject burnVFX;

    public override float GetDamage() => wrappedSpell.GetDamage() + bonusDamage;
    public override float GetManaCost() => wrappedSpell.GetManaCost() + bonusManaCost;

    public override void Cast(Vector2 direction, Vector2 origin)
    {
        if (burnProjectilePrefab != null)
        {
            GameObject proj = Instantiate(burnProjectilePrefab, origin, Quaternion.identity);
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
        StartCoroutine(ApplyBurn(target));
    }

    private IEnumerator ApplyBurn(GameObject target)
    {
        if (burnVFX != null)
        {
            GameObject vfx = Instantiate(burnVFX, target.transform.position, Quaternion.identity);
            vfx.transform.SetParent(target.transform);
            Destroy(vfx, burnDuration);
        }

        float elapsed = 0f;
        while (elapsed < burnDuration)
        {
            yield return new WaitForSeconds(burnTickRate);
            elapsed += burnTickRate;

            if (target == null) yield break;

            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable == null || damageable.IsDead) yield break;

            damageable.TakeDamage(burnDamage);
        }
    }
}