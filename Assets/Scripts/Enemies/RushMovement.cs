using UnityEngine;

public class RushMovement : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float speed = 3f;
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
        Vector2 direction = (target.position - self.position).normalized;
        self.position = Vector2.MoveTowards(self.position,
            target.position, speed * Time.deltaTime);
    }
}