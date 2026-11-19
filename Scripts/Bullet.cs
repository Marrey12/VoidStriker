using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 12f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;

    private bool hitSomething = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(
            Vector2.up * bulletSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth =
            other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            hitSomething = true;

            AchievementManager.Instance?.RegisterHit();

            enemyHealth.TakeDamage(damage);

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!hitSomething)
        {
            AchievementManager.Instance?.RegisterMiss();
        }
    }
}
