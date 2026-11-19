using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthBar;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private GameObject waveCompletedText;

    [SerializeField] private Slider bossHealthBar;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    private PlayerHealth player;
    private EnemySpawner enemySpawner;
    private EnemyHealth currentBossHealth;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        enemySpawner = FindFirstObjectByType<EnemySpawner>();

        if (waveCompletedText != null)
        {
            waveCompletedText.SetActive(false);
        }

        if (bossHealthBar != null)
        {
            bossHealthBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        scoreText.text =
            "SCORE: " + GameManager.Instance.Score;

        UpdateHealthUI();
        UpdateWaveUI();
        UpdateBossHealthBar();
    }

    private void UpdateHealthUI()
    {
        if (player == null) return;

        float hpPercent =
            Mathf.Clamp(
                (player.CurrentHealth / player.MaxHealth) * 100f,
                0f,
                100f
            );

        healthText.text =
            "HP: " + hpPercent.ToString("0") + "%";

        healthBar.value =
            player.CurrentHealth / player.MaxHealth;
    }

    private void UpdateWaveUI()
    {
        if (enemySpawner == null) return;

        waveText.text =
            "WAVE: " + enemySpawner.CurrentWave;

        int seconds =
            Mathf.CeilToInt(enemySpawner.WaveTimer);

        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        waveTimerText.text =
            "TIME: " +
            minutes.ToString("00") +
            ":" +
            remainingSeconds.ToString("00");
    }

    private void UpdateBossHealthBar()
    {
        if (currentBossHealth == null || bossHealthBar == null) return;

        bossHealthBar.value =
            (float)currentBossHealth.CurrentHealth /
            currentBossHealth.MaxHealth;
    }

    public void ShowBossHealthBar(EnemyHealth bossHealth)
    {
        currentBossHealth = bossHealth;

        if (bossHealthBar != null)
        {
            bossHealthBar.gameObject.SetActive(true);
            bossHealthBar.value = 1f;
        }
    }

    public void HideBossHealthBar()
    {
        currentBossHealth = null;

        if (bossHealthBar != null)
        {
            bossHealthBar.gameObject.SetActive(false);
        }
    }

    public void ShowWaveCompleted()
    {
        AudioManager.Instance?.PlayWaveCompleted();
        if (waveCompletedText == null) return;

        waveCompletedText.SetActive(true);

        CancelInvoke(nameof(HideWaveCompleted));
        Invoke(nameof(HideWaveCompleted), 2f);
    }

    private void HideWaveCompleted()
    {
        if (waveCompletedText != null)
        {
            waveCompletedText.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        finalScoreText.text =
            "FINAL SCORE: " +
            GameManager.Instance.Score;
    }
}
