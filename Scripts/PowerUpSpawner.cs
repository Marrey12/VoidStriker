using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healPowerUpPrefab;
    [SerializeField] private GameObject speedPowerUpPrefab;
    [SerializeField] private GameObject rapidFirePowerUpPrefab;

    [SerializeField] private float spawnInterval = 12f;

    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnPowerUp();

            nextSpawnTime =
                Time.time + spawnInterval;
        }
    }

    private void SpawnPowerUp()
    {
        float randomX = Random.Range(-7f, 7f);
        float randomY = Random.Range(-3f, 3f);

        Vector3 spawnPosition =
            new Vector3(randomX, randomY, 0f);

        int randomPowerUp =
            Random.Range(0, 3);

        if (randomPowerUp == 0)
        {
            Instantiate(
                healPowerUpPrefab,
                spawnPosition,
                Quaternion.identity
            );
        }
        else if (randomPowerUp == 1)
        {
            Instantiate(
                speedPowerUpPrefab,
                spawnPosition,
                Quaternion.identity
            );
        }
        else
        {
            Instantiate(
                rapidFirePowerUpPrefab,
                spawnPosition,
                Quaternion.identity
            );
        }
    }
}
