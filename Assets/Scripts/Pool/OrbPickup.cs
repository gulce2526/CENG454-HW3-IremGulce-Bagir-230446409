using UnityEngine;

public class OrbPickup : MonoBehaviour, IPoolable
{
    [SerializeField] private int scoreValue = 10;
    private ObjectPool myPool;

    public void SetPool(ObjectPool pool)
    {
        myPool = pool;
    }

    public void SetScoreValue(int value)
    {
        scoreValue = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        GameEvents.OrbCollected(scoreValue);

        if (myPool != null)
            myPool.Return(gameObject);
        else
            gameObject.SetActive(false);
    }

    public void OnSpawn() { }

    public void OnReturnToPool() { }
}