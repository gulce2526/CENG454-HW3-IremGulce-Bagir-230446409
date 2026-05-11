using UnityEngine;

public class EnemyProjectilePool : MonoBehaviour
{
    public static EnemyProjectilePool Instance;

    [SerializeField] private ObjectPool arrowPool;

    private void Awake()
    {
        Instance = this;
    }

    public ObjectPool GetPool() => arrowPool;
}