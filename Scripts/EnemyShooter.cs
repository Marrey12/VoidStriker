using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletDamage = 10f;

    [SerializeField] private Color bulletColor = Color.white;

    private Transform player;
    private float nextFireTime;

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Vector2 direction =
            player.position - firePoint.position;

        EnemyBullet enemyBullet =
            bullet.GetComponent<EnemyBullet>();

        enemyBullet.SetDirection(direction);
        enemyBullet.SetDamage(bulletDamage);
        enemyBullet.SetColor(bulletColor);
        AudioManager.Instance?.PlayEnemyShoot();
    }
}
