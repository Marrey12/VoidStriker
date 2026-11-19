using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int scoreValue = 10;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        if (GetComponent<BossController>() != null)
        {
            FindFirstObjectByType<UIManager>()?.ShowBossHealthBar(this);
        }
    }

    public void TakeDamage(int damage)
    {
        BerserkEnemy berserkEnemy = GetComponent<BerserkEnemy>();

        if (berserkEnemy != null)
        {
            berserkEnemy.ActivateBerserk();
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance?.PlayEnemyDeath();

        AchievementManager.Instance?.RegisterEnemyKilled();

        GameManager.Instance.AddScore(scoreValue);

        BossController bossController = GetComponent<BossController>();

        if (bossController != null)
        {
            PlayerHealth player =
                FindFirstObjectByType<PlayerHealth>();

            if (player != null)
            {
                float hpPercent =
                    (player.CurrentHealth / player.MaxHealth) * 100f;

                AchievementManager.Instance?.RegisterBossKilledLowHp(
                    hpPercent
                );
            }

            FindFirstObjectByType<UIManager>()?.HideBossHealthBar();
            bossController.NotifyBossDefeated();
        }

        Destroy(gameObject);
    }
}
