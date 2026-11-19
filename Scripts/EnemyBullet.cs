using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float lifetime = 4f;

    private float damage = 10f;
    private Vector2 moveDirection;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(
            moveDirection * speed * Time.deltaTime,
            Space.World
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
