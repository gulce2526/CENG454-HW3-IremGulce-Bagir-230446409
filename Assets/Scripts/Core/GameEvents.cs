using System;

public static class GameEvents
{
    public static event Action<float> OnCoreDamaged;
    public static event Action OnCoreDestroyed;
    public static event Action<int> OnEnemyKilled;
    public static event Action OnWaveCompleted;
    public static event Action OnGameWon;
    public static event Action<int> OnOrbCollected;
    public static event Action<int> OnSpellUnlocked;

    public static void CoreDamaged(float amount) => OnCoreDamaged?.Invoke(amount);
    public static void CoreDestroyed() => OnCoreDestroyed?.Invoke();
    public static void EnemyKilled(int points) => OnEnemyKilled?.Invoke(points);
    public static void WaveCompleted() => OnWaveCompleted?.Invoke();
    public static void GameWon() => OnGameWon?.Invoke();
    public static void OrbCollected(int score) => OnOrbCollected?.Invoke(score);
    public static void SpellUnlocked(int spellLevel) => OnSpellUnlocked?.Invoke(spellLevel);
}