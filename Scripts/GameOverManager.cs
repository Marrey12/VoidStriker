using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private bool gameEnded = false;

    private void Update()
    {
        if (gameEnded && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void ShowGameOver()
    {
        gameEnded = true;

        gameOverPanel.SetActive(true);

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
