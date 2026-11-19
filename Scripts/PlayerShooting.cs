using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private enum WeaponType
    {
        Normal,
        Double,
        Spread
    }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireRate = 0.25f;

    [Header("Double Shot")]
    [SerializeField] private float doubleShotOffset = 0.25f;

    [Header("Spread Shot")]
    [SerializeField] private float spreadAngle = 20f;

    [Header("Area Shot")]
    [SerializeField] private float areaShotRadius = 3f;
    [SerializeField] private int areaShotDamage = 3;
    [SerializeField] private float areaShotCooldown = 20f;

    [Header("Special Weapon Timing")]
    [SerializeField] private float specialWeaponDuration = 10f;
    [SerializeField] private float specialWeaponCooldown = 20f;

    private WeaponType currentWeapon = WeaponType.Normal;

    private float currentFireRate;
    private float rapidFireTimer = 0f;
    private float nextFireTime;

    private float specialWeaponTimer = 0f;

    private float doubleCooldownTimer = 0f;
    private float spreadCooldownTimer = 0f;
    private float areaCooldownTimer = 0f;

    public float DoubleCooldown => Mathf.Max(0f, doubleCooldownTimer);
    public float SpreadCooldown => Mathf.Max(0f, spreadCooldownTimer);
    public float AreaCooldown => Mathf.Max(0f, areaCooldownTimer);

    private void Start()
    {
        currentFireRate = fireRate;
    }

    private void Update()
    {
        HandleCooldowns();
        HandleWeaponSwitching();
        UpdateSpecialWeaponTimer();
        UpdateRapidFire();

        if (Input.GetKey(KeyCode.Space) &&
            Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + currentFireRate;
        }
    }

    private void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WeaponType.Normal;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) &&
            doubleCooldownTimer <= 0f)
        {
            currentWeapon = WeaponType.Double;
            specialWeaponTimer = specialWeaponDuration;
            doubleCooldownTimer =
                specialWeaponDuration + specialWeaponCooldown;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) &&
            spreadCooldownTimer <= 0f)
        {
            currentWeapon = WeaponType.Spread;
            specialWeaponTimer = specialWeaponDuration;
            spreadCooldownTimer =
                specialWeaponDuration + specialWeaponCooldown;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) &&
            areaCooldownTimer <= 0f)
        {
            UseAreaShot();
            areaCooldownTimer = areaShotCooldown;
            currentWeapon = WeaponType.Normal;
        }
    }

    private void UpdateSpecialWeaponTimer()
    {
        if (currentWeapon == WeaponType.Normal) return;

        specialWeaponTimer -= Time.deltaTime;

        if (specialWeaponTimer <= 0f)
        {
            currentWeapon = WeaponType.Normal;
        }
    }

    private void HandleCooldowns()
    {
        if (doubleCooldownTimer > 0f)
            doubleCooldownTimer -= Time.deltaTime;

        if (spreadCooldownTimer > 0f)
            spreadCooldownTimer -= Time.deltaTime;

        if (areaCooldownTimer > 0f)
            areaCooldownTimer -= Time.deltaTime;
    }

    private void Shoot()
    {
        if (currentWeapon == WeaponType.Normal)
        {
            ShootNormal();
        }
        else if (currentWeapon == WeaponType.Double)
        {
            ShootDouble();
        }
        else if (currentWeapon == WeaponType.Spread)
        {
            ShootSpread();
        }

        AudioManager.Instance?.PlayPlayerShoot();
    }

    private void ShootNormal()
    {
        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );
    }

    private void ShootDouble()
    {
        Vector3 leftPosition =
            firePoint.position +
            new Vector3(-doubleShotOffset, 0f, 0f);

        Vector3 rightPosition =
            firePoint.position +
            new Vector3(doubleShotOffset, 0f, 0f);

        Instantiate(bulletPrefab, leftPosition, firePoint.rotation);
        Instantiate(bulletPrefab, rightPosition, firePoint.rotation);
    }

    private void ShootSpread()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.Euler(0f, 0f, spreadAngle)
        );

        Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.Euler(0f, 0f, -spreadAngle)
        );
    }

    private void UseAreaShot()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                areaShotRadius
            );

        int killedByAreaShot = 0;

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHealth =
                hit.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                int healthBeforeHit =
                    enemyHealth.CurrentHealth;

                enemyHealth.TakeDamage(areaShotDamage);

                if (healthBeforeHit > 0 &&
                    enemyHealth.CurrentHealth <= 0)
                {
                    killedByAreaShot++;
                }
            }
        }

        AchievementManager.Instance?.RegisterOneShotKill(
            killedByAreaShot
        );
    }

    private void UpdateRapidFire()
    {
        if (rapidFireTimer > 0f)
        {
            rapidFireTimer -= Time.deltaTime;

            if (rapidFireTimer <= 0f)
            {
                currentFireRate = fireRate;
            }
        }
    }

    public void ActivateRapidFire(
        float newFireRate,
        float duration)
    {
        currentFireRate = newFireRate;
        rapidFireTimer = duration;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, areaShotRadius);
    }
}
