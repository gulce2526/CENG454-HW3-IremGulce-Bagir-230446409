using UnityEngine;

public class ArcherMovement : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float preferredDistance = 6f;
    [SerializeField] private float speed = 2f;
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
        float currentDistance = Vector2.Distance(self.position, target.position);

        if (currentDistance > preferredDistance)
        {
            self.position = Vector2.MoveTowards(self.position,
                target.position, speed * Time.deltaTime);
        }
        else if (currentDistance < preferredDistance - 0.5f)
        {
            self.position = Vector2.MoveTowards(self.position,
                target.position, -speed * Time.deltaTime);
        }
    }
}