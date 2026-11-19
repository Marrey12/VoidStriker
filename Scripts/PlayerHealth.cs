using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        AchievementManager.Instance?.RegisterPlayerDamage();

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        AudioManager.Instance?.PlayPlayerHit();

        if (currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void SetHealth(float value)
    {
        currentHealth =
            Mathf.Clamp(value, 0f, maxHealth);
    }
}
