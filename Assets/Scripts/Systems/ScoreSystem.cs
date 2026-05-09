using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private int totalScore = 0;

    [Header("Spell Unlock Thresholds")]
    [SerializeField] private int frostUnlockScore = 50;
    [SerializeField] private int loveUnlockScore = 120;
    [SerializeField] private int burnUnlockScore = 200;

    private bool frostUnlocked = false;
    private bool loveUnlocked = false;
    private bool burnUnlocked = false;

    private void OnEnable()
    {
        GameEvents.OnOrbCollected += HandleOrbCollected;
    }

    private void OnDisable()
    {
        GameEvents.OnOrbCollected -= HandleOrbCollected;
    }

    private void HandleOrbCollected(int points)
    {
        totalScore += points;

        if (!frostUnlocked && totalScore >= frostUnlockScore)
        {
            frostUnlocked = true;
            GameEvents.SpellUnlocked(1);
            Debug.Log("Frost spell unlocked!");
        }

        if (!loveUnlocked && totalScore >= loveUnlockScore)
        {
            loveUnlocked = true;
            GameEvents.SpellUnlocked(2);
            Debug.Log("Love spell unlocked!");
        }

        if (!burnUnlocked && totalScore >= burnUnlockScore)
        {
            burnUnlocked = true;
            GameEvents.SpellUnlocked(3);
            Debug.Log("Burn spell unlocked!");
        }
    }

    public int GetTotalScore() => totalScore;
}