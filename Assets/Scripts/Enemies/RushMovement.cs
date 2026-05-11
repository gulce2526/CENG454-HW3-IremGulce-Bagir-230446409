using UnityEngine;
using Pathfinding; 

[RequireComponent(typeof(AIPath))] 
public class RushMovement : MonoBehaviour, IMovementStrategy
{
    [SerializeField] private float speed = 3f;
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
       
        aiPath.isStopped = false;

        
        aiPath.destination = target.position;
    }
}