using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;
    private bool isGameOver = false;

    public int Score => score;
    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameplayMusic();
        }
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
    }

    public void SetScore(int value)
    {
        score = Mathf.Max(0, value);
    }

    public void GameOver()
    {
        AudioManager.Instance?.PlayGameOver();
        if (isGameOver) return;

        isGameOver = true;

        HighScoreManager.AddScore(
            PlayerProfileManager.CurrentPlayerId,
            score
        );

        FindFirstObjectByType<UIManager>()
            .ShowGameOver();

        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
