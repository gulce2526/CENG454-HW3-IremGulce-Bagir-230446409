using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI panelTitleText; // "GAME OVER" or "VICTORY!"
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Systems")]
    [SerializeField] private ScoreSystem scoreSystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Show start panel and pause game until player clicks Start
        if (gameStartPanel != null)
        {
            gameStartPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnEnable()
    {
        GameEvents.OnCoreDestroyed += HandleGameOver;
        GameEvents.OnGameWon += HandleGameWon;
        GameEvents.OnPlayerDied += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnCoreDestroyed -= HandleGameOver;
        GameEvents.OnGameWon -= HandleGameWon;
        GameEvents.OnPlayerDied -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // Wait for death animation
        yield return new WaitForSeconds(2f);

        // Set title to game over
        if (panelTitleText != null)
        {
            panelTitleText.text = "GAME OVER";
        }

        // Update final score
        if (scoreText != null && scoreSystem != null)
        {
            scoreText.text = $"Final Score: {scoreSystem.GetTotalScore()}";
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void HandleGameWon()
    {
        // Change title to victory message
        if (panelTitleText != null)
        {
            panelTitleText.text = "VICTORY!";
        }

        // Update final score
        if (scoreText != null && scoreSystem != null)
        {
            scoreText.text = $"Final Score: {scoreSystem.GetTotalScore()}";
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        if (gameStartPanel != null)
        {
            gameStartPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}