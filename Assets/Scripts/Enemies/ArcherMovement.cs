using UnityEngine;
using Pathfinding; 

[RequireComponent(typeof(AIPath))]
public class ArcherMovement : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float preferredDistance = 6f;
    [SerializeField] private float speed = 2f;
    private float originalSpeed;

    
    private AIPath aiPath;

    private void Start()
    {
        originalSpeed = speed;
        aiPath = GetComponent<AIPath>();

        
        aiPath.maxSpeed = speed;
        aiPath.enableRotation = false; 
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        aiPath.maxSpeed = originalSpeed * multiplier;
    }

    public void Move(Transform self, Transform target)
    {
        
        float currentDistance = Vector2.Distance(self.position, target.position);

        if (currentDistance > preferredDistance)
        {
            
            aiPath.isStopped = false;
            aiPath.destination = target.position;
        }
        else if (currentDistance < preferredDistance - 0.5f)
        {
            
            aiPath.isStopped = false;

           
            Vector3 directionAway = (self.position - target.position).normalized;

            
            aiPath.destination = self.position + (directionAway * 2f);
        }
        else
        {
            
            aiPath.isStopped = true;
        }
    }
}