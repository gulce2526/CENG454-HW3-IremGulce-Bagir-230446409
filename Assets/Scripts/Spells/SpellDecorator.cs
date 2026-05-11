using UnityEngine;

public abstract class SpellDecorator : MonoBehaviour, ISpellEffect
{
    protected ISpellEffect wrappedSpell;

    public void SetWrappedSpell(ISpellEffect spell)
    {
        wrappedSpell = spell;
    }

    public virtual float GetDamage() => wrappedSpell.GetDamage();
    public virtual float GetManaCost() => wrappedSpell.GetManaCost();
    public virtual float GetRange() => wrappedSpell.GetRange();

    public virtual void Cast(Vector2 direction, Vector2 origin)
    {
        wrappedSpell.Cast(direction, origin);
    }

    public virtual void Apply(GameObject target)
    {
        wrappedSpell.Apply(target);
    }
}