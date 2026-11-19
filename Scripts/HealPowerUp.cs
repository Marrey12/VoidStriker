using UnityEngine;

public class HealPowerUp : MonoBehaviour
{
    [SerializeField] private float healAmount = 20f;
    [SerializeField] private float lifetime = 8f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }
            AudioManager.Instance?.PlayHealPowerUp();
            Destroy(gameObject);
        }
    }
}
