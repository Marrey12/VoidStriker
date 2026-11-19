using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float collisionDamage = 20f;
    [SerializeField] private float baseDamage = 10f;

    private void Update()
    {
        transform.Translate(
            Vector2.down * moveSpeed * Time.deltaTime
        );

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(collisionDamage);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("PlayerBase"))
        {
            PlayerHealth playerHealth =
                FindFirstObjectByType<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(baseDamage);
            }

            Destroy(gameObject);
        }
    }
}
