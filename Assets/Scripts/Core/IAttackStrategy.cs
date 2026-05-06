using UnityEngine;

public interface IAttackStrategy
{
    void Attack(Transform self, Transform target);
}