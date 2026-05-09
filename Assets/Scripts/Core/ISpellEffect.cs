using UnityEngine;

public interface ISpellEffect
{
    float GetDamage();
    float GetManaCost();
    float GetRange();
    void Cast(Vector2 direction, Vector2 origin);
    void Apply(GameObject target);
}