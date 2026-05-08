using UnityEngine;

public interface IMovementStrategy
{
    void Move(Transform self, Transform target);
}