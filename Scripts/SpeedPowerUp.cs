using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1.8f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float lifetime = 8f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement =
                other.GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.ActivateSpeedBoost(
                    speedMultiplier,
                    duration
                );
            }
            AudioManager.Instance?.PlaySpeedPowerUp();
            Destroy(gameObject);
        }
    }
}
