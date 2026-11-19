using UnityEngine;

public class RapidFirePowerUp : MonoBehaviour
{
    [SerializeField] private float boostedFireRate = 0.08f;
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
            PlayerShooting playerShooting =
                other.GetComponent<PlayerShooting>();

            if (playerShooting != null)
            {
                playerShooting.ActivateRapidFire(
                    boostedFireRate,
                    duration
                );
            }
            AudioManager.Instance?.PlayRapidFirePowerUp();
            Destroy(gameObject);
        }
    }
}
