using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HUDController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Mana")]
    [SerializeField] private Slider manaBar;
    [SerializeField] private TextMeshProUGUI manaText;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Spells")]
    [SerializeField] private Image[] spellSlots;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    [Header("RuneStone")]
    [SerializeField] private Slider runeStoneHealthBar;
    [SerializeField] private RuneStone runeStone;

    [Header("Notifications")]
    [SerializeField] private TextMeshProUGUI unlockMessageText;

    [Header("References")]
    [SerializeField] private PlayerController player;

    private int currentScore = 0;
    private int selectedSpellIndex = 0;
    private int unlockedSpellCount = 1;

    private void OnEnable()
    {
        GameEvents.OnOrbCollected += HandleOrbCollected;
        GameEvents.OnSpellUnlocked += HandleSpellUnlocked;
        GameEvents.OnCoreDamaged += HandleCoreDamaged;
    }

    private void OnDisable()
    {
        GameEvents.OnOrbCollected -= HandleOrbCollected;
        GameEvents.OnSpellUnlocked -= HandleSpellUnlocked;
        GameEvents.OnCoreDamaged -= HandleCoreDamaged;
    }

    private void Start()
    {
        if (unlockMessageText != null)
            unlockMessageText.gameObject.SetActive(false);

        if (runeStone != null && runeStoneHealthBar != null)
            runeStoneHealthBar.value = 1f;
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateManaBar();
        UpdateSpellSlots();
    }

    private void UpdateHealthBar()
    {
        if (player == null || healthBar == null) return;
        healthBar.value = player.CurrentHealth / player.MaxHealth;
        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(player.CurrentHealth)}/{player.MaxHealth}";
    }

    private void UpdateManaBar()
    {
        if (player == null || manaBar == null) return;
        manaBar.value = player.CurrentMana / player.MaxMana;
        if (manaText != null)
            manaText.text = $"{Mathf.CeilToInt(player.CurrentMana)}/{player.MaxMana}";
    }

    private void HandleOrbCollected(int points)
    {
        currentScore += points;
        if (scoreText != null)
            scoreText.text = $"Score: {currentScore}";
    }

    private void HandleSpellUnlocked(int spellLevel)
    {
        unlockedSpellCount = spellLevel + 1;
        selectedSpellIndex = spellLevel;
        UpdateSpellSlots();

        string[] names = { "Base Spell", "Frost Spell", "Love Spell", "Burn Spell" };
        if (spellLevel < names.Length)
            StartCoroutine(ShowUnlockMessage($"{names[spellLevel]} Unlocked!\nPress {spellLevel + 1} to use"));
    }

    private void HandleCoreDamaged(float currentHealth)
    {
        if (runeStoneHealthBar != null && runeStone != null)
            runeStoneHealthBar.value = currentHealth / runeStone.MaxHealth;
    }

    private void UpdateSpellSlots()
    {
        if (spellSlots == null) return;

        for (int i = 0; i < spellSlots.Length; i++)
        {
            if (spellSlots[i] == null) continue;

            if (i == selectedSpellIndex)
                spellSlots[i].color = selectedColor;
            else if (i < unlockedSpellCount)
                spellSlots[i].color = unlockedColor;
            else
                spellSlots[i].color = lockedColor;
        }
    }

    private IEnumerator ShowUnlockMessage(string msg)
    {
        if (unlockMessageText == null) yield break;

        unlockMessageText.text = msg;
        unlockMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        unlockMessageText.gameObject.SetActive(false);
    }
}