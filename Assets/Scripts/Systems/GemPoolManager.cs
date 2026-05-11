using UnityEngine;

public class GemPoolManager : MonoBehaviour
{
    public static GemPoolManager Instance;

    [SerializeField] private ObjectPool greenGemPool;
    [SerializeField] private ObjectPool blueGemPool;
    [SerializeField] private ObjectPool purpleGemPool;

    private void Awake()
    {
        Instance = this;
    }

    public ObjectPool GetPool(GemType type)
    {
        switch (type)
        {
            case GemType.Green: return greenGemPool;
            case GemType.Blue: return blueGemPool;
            case GemType.Purple: return purpleGemPool;
            default: return null;
        }
    }
}

public enum GemType { Green, Blue, Purple }