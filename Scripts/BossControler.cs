using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    [Header("Boss Movement Area")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;
    [SerializeField] private float minY = 1.2f;
    [SerializeField] private float maxY = 4f;
    [SerializeField] private float changeTargetDistance = 0.2f;

    [Header("Bullet Hell Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 16;
    [SerializeField] private float bulletHellInterval = 8f;
    [SerializeField] private float bulletHellChargeTime = 2f;
    [SerializeField] private float bulletDamage = 8f;

    [Header("Laser Attack")]
    [SerializeField] private GameObject laserVisual;
    [SerializeField] private float laserInterval = 12f;
    [SerializeField] private float laserChargeTime = 2f;
    [SerializeField] private float laserActiveTime = 0.3f;
    [SerializeField] private float laserDamage = 25f;
    [SerializeField] private float laserWidth = 1.2f;
    [SerializeField] private float laserLength = 10f;

    [Header("Drone Summon Attack")]
    [SerializeField] private GameObject droneEnemyPrefab;
    [SerializeField] private int minDrones = 2;
    [SerializeField] private int maxDrones = 4;
    [SerializeField] private float droneSummonInterval = 15f;
    [SerializeField] private float droneChargeTime = 1.5f;

    [Header("Meteor Shower Attack")]
    [SerializeField] private GameObject meteorMarkerPrefab;
    [SerializeField] private float meteorInterval = 18f;
    [SerializeField] private float meteorChargeTime = 2f;
    [SerializeField] private int meteorCount = 5;
    [SerializeField] private float meteorDamage = 20f;
    [SerializeField] private float meteorRadius = 0.7f;

    private Vector3 targetPosition;

    private float nextBulletHellTime;
    private float nextLaserTime;
    private float nextDroneSummonTime;
    private float nextMeteorTime;

    private bool isCharging = false;

    private void Start()
    {
        ChooseNewTargetPosition();

        nextBulletHellTime = Time.time + bulletHellInterval;
        nextLaserTime = Time.time + laserInterval;
        nextDroneSummonTime = Time.time + droneSummonInterval;
        nextMeteorTime = Time.time + meteorInterval;

        if (laserVisual != null)
        {
            laserVisual.SetActive(false);
        }
    }

    private void Update()
    {
        MoveInsideBossArea();
        HandleSpecialAttacks();
    }

    private void MoveInsideBossArea()
    {
        if (isCharging) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition)
            <= changeTargetDistance)
        {
            ChooseNewTargetPosition();
        }
    }

    private void ChooseNewTargetPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        targetPosition = new Vector3(randomX, randomY, 0f);
    }

    private void HandleSpecialAttacks()
    {
        if (isCharging) return;

        if (Time.time >= nextLaserTime)
        {
            StartCoroutine(ChargeAndFireLaser());
            return;
        }

        if (Time.time >= nextBulletHellTime)
        {
            StartCoroutine(ChargeAndShootBulletHell());
            return;
        }

        if (Time.time >= nextMeteorTime)
        {
            StartCoroutine(ChargeAndDropMeteors());
            return;
        }

        if (Time.time >= nextDroneSummonTime)
        {
            StartCoroutine(ChargeAndSummonDrones());
        }
    }

    private System.Collections.IEnumerator ChargeAndShootBulletHell()
    {
        AudioManager.Instance?.PlayBossAttack();

        isCharging = true;

        yield return new WaitForSeconds(bulletHellChargeTime);

        ShootBulletHell();

        isCharging = false;
        nextBulletHellTime = Time.time + bulletHellInterval;
    }

    private void ShootBulletHell()
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;

            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            EnemyBullet enemyBullet =
                bullet.GetComponent<EnemyBullet>();

            if (enemyBullet != null)
            {
                enemyBullet.SetDirection(direction);
                enemyBullet.SetDamage(bulletDamage);
            }
        }
    }

    private System.Collections.IEnumerator ChargeAndFireLaser()
    {
        AudioManager.Instance?.PlayBossAttack();

        isCharging = true;

        ShowLaserWarning();

        yield return new WaitForSeconds(laserChargeTime);

        ShowLaserFull();
        FireLaser();

        yield return new WaitForSeconds(laserActiveTime);

        HideLaser();

        isCharging = false;
        nextLaserTime = Time.time + laserInterval;
    }

    private void ShowLaserWarning()
    {
        if (laserVisual == null) return;

        laserVisual.SetActive(true);
        laserVisual.transform.localPosition =
            new Vector3(0f, -laserLength / 2f, 0f);

        laserVisual.transform.localScale =
            new Vector3(laserWidth * 0.25f, laserLength, 1f);
    }

    private void ShowLaserFull()
    {
        if (laserVisual == null) return;

        laserVisual.transform.localPosition =
            new Vector3(0f, -laserLength / 2f, 0f);

        laserVisual.transform.localScale =
            new Vector3(laserWidth, laserLength, 1f);
    }

    private void HideLaser()
    {
        if (laserVisual != null)
        {
            laserVisual.SetActive(false);
        }
    }

    private void FireLaser()
    {
        Vector2 laserCenter =
            transform.position + Vector3.down * (laserLength / 2f);

        Vector2 laserSize =
            new Vector2(laserWidth, laserLength);

        Collider2D[] hits =
            Physics2D.OverlapBoxAll(
                laserCenter,
                laserSize,
                0f
            );

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth =
                    hit.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(laserDamage);
                }
            }
        }
    }

    private System.Collections.IEnumerator ChargeAndSummonDrones()
    {
        AudioManager.Instance?.PlayBossAttack();

        isCharging = true;

        yield return new WaitForSeconds(droneChargeTime);

        SummonDrones();

        isCharging = false;
        nextDroneSummonTime = Time.time + droneSummonInterval;
    }

    private void SummonDrones()
    {
        if (droneEnemyPrefab == null) return;

        int droneCount =
            Random.Range(minDrones, maxDrones + 1);

        for (int i = 0; i < droneCount; i++)
        {
            float randomX = Random.Range(-5.8f, 5.8f);
            float randomY = Random.Range(2f, 4.5f);

            Vector3 spawnPosition =
                new Vector3(randomX, randomY, 0f);

            Instantiate(
                droneEnemyPrefab,
                spawnPosition,
                Quaternion.identity
            );
        }
    }

    private System.Collections.IEnumerator ChargeAndDropMeteors()
    {
        AudioManager.Instance?.PlayBossAttack();

        isCharging = true;

        Vector3[] meteorPositions =
            new Vector3[meteorCount];

        GameObject[] markers =
            new GameObject[meteorCount];

        for (int i = 0; i < meteorCount; i++)
        {
            float randomX = Random.Range(-5.8f, 5.8f);
            float randomY = Random.Range(-3.5f, 3.5f);

            meteorPositions[i] =
                new Vector3(randomX, randomY, 0f);

            markers[i] = Instantiate(
                meteorMarkerPrefab,
                meteorPositions[i],
                Quaternion.identity
            );
        }

        yield return new WaitForSeconds(meteorChargeTime);

        for (int i = 0; i < meteorPositions.Length; i++)
        {
            DealMeteorDamage(meteorPositions[i]);

            if (markers[i] != null)
            {
                Destroy(markers[i]);
            }
        }

        isCharging = false;
        nextMeteorTime = Time.time + meteorInterval;
    }

    private void DealMeteorDamage(Vector3 position)
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                position,
                meteorRadius
            );

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth =
                    hit.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(meteorDamage);
                }
            }
        }
    }

    public void NotifyBossDefeated()
    {
        EnemySpawner spawner =
            FindFirstObjectByType<EnemySpawner>();

        if (spawner != null)
        {
            spawner.OnBossDefeated();
        }
    }
}
