using UnityEngine;

public class BruteMovement : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float speed = 1f;
    private float originalSpeed;

    private void Start()
    {
        originalSpeed = speed;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speed = originalSpeed * multiplier;
    }

    public void Move(Transform self, Transform target)
    {
        self.position = Vector2.MoveTowards(self.position,
            target.position, speed * Time.deltaTime);
    }
}