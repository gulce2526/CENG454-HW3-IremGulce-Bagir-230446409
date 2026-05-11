using UnityEngine;

public class AltarOfHealing : MonoBehaviour
{
    [Header("Regeneration Rates (Per Second)")]
    [SerializeField] private float healRate = 10f;
    [SerializeField] private float manaRate = 15f;

    private bool isPlayerOnAltar = false;
    private PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnAltar = true;
            player = collision.GetComponent<PlayerController>();

            
            Debug.Log("Player entered safe zone. Healing started.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnAltar = false;
            player = null;

          
            Debug.Log("Player left safe zone. Healing stopped.");
        }
    }

    private void Update()
    {
        // If the player is standing on the altar, continuously give them health and mana
        if (isPlayerOnAltar && player != null)
        {
            // Time.deltaTime ensures they regenerate smoothly over time, not instantly!
            player.Heal(healRate * Time.deltaTime);
            player.AddMana(manaRate * Time.deltaTime);
        }
    }
}