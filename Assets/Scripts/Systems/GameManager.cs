using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWonPanel;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
        if (gameEnded) return;
        gameEnded = true;

        StartCoroutine(GameOverDelayed());
    }

    private System.Collections.IEnumerator GameOverDelayed()
    {
        yield return new WaitForSeconds(2f);  // wait for die animation
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Debug.Log("Game Over!");
    }

    private void HandleGameWon()
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 0f;

        if (gameWonPanel != null)
            gameWonPanel.SetActive(true);

        Debug.Log("You won! All waves defeated!");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}