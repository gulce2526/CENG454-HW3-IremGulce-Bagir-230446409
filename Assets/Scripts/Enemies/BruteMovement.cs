using UnityEngine;

public class BruteMovement : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float speed = 1f;

    public void Move(Transform self, Transform target)
    {
        self.position = Vector2.MoveTowards(self.position,
            target.position, speed * Time.deltaTime);
    }
}