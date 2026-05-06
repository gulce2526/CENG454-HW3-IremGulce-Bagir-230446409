using UnityEngine;

public interface ISpellEffect
{
    float GetDamage();
    void Apply(GameObject target);
}