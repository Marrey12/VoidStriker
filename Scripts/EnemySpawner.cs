using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;
    [SerializeField] private GameObject rapidEnemy;
    [SerializeField] private GameObject tankEnemy;
    [SerializeField] private GameObject berserkEnemy;

    [SerializeField] private GameObject bossEnemy;

    [SerializeField] private float baseWaveDuration = 60f;
    [SerializeField] private float waveDurationIncrease = 30f;
    [SerializeField] private float timeBetweenWaves = 3f;
    [SerializeField] private float spawnRate = 1.2f;

    private int currentWave = 1;
    private float waveTimer;
    private float nextSpawnTime;
    private bool waveActive = false;
    private bool bossFightActive = false;

    public int CurrentWave => currentWave;
    public float WaveTimer => waveTimer;
    public bool WaveActive => waveActive;

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;
        if (bossFightActive) return;
        if (!waveActive) return;

        waveTimer -= Time.deltaTime;

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }

        if (waveTimer <= 0f)
        {
            CompleteWave();
        }
    }

    private void StartWave()
    {
        bossFightActive = false;

        waveTimer =
            baseWaveDuration +
            (currentWave - 1) * waveDurationIncrease;

        waveActive = true;
        nextSpawnTime = Time.time + 1f;
    }

    private void CompleteWave()
    {
        waveActive = false;
        waveTimer = 0f;

        UIManager uiManager =
            FindFirstObjectByType<UIManager>();

        if (uiManager != null)
        {
            uiManager.ShowWaveCompleted();
        }

        if (currentWave % 5 == 0)
        {
            Invoke(nameof(StartBossFight), timeBetweenWaves);
        }
        else
        {
            Invoke(nameof(NextWave), timeBetweenWaves);
        }
    }

    private void StartBossFight()
    {
        bossFightActive = true;
        waveActive = false;

        Vector3 bossSpawnPosition =
            new Vector3(0f, 3.5f, 0f);

        Instantiate(
            bossEnemy,
            bossSpawnPosition,
            Quaternion.identity
        );
    }

    public void OnBossDefeated()
    {
        bossFightActive = false;

        UIManager uiManager =
            FindFirstObjectByType<UIManager>();

        if (uiManager != null)
        {
            uiManager.ShowWaveCompleted();
        }

        Invoke(nameof(NextWave), timeBetweenWaves);
    }

    private void NextWave()
    {
        currentWave++;
        StartWave();
    }

    private void SpawnEnemy()
    {
        float randomX =
            Random.Range(-5.8f, 5.8f);

        Vector3 spawnPosition =
            new Vector3(randomX, 6f, 0f);

        Instantiate(
            ChooseEnemyForWave(),
            spawnPosition,
            Quaternion.identity
        );
        AudioManager.Instance?.PlayEnemySpawn();
    }

    private GameObject ChooseEnemyForWave()
    {
        int wavePattern =
            ((currentWave - 1) % 5) + 1;

        if (wavePattern == 1)
        {
            return basicEnemy;
        }

        if (wavePattern == 2)
        {
            return Random.value < 0.7f
                ? basicEnemy
                : fastEnemy;
        }

        if (wavePattern == 3)
        {
            int random = Random.Range(0, 3);

            if (random == 0) return basicEnemy;
            if (random == 1) return fastEnemy;
            return rapidEnemy;
        }

        if (wavePattern == 4)
        {
            int random = Random.Range(0, 4);

            if (random == 0) return basicEnemy;
            if (random == 1) return fastEnemy;
            if (random == 2) return rapidEnemy;
            return tankEnemy;
        }

        int finalRandom = Random.Range(0, 5);

        if (finalRandom == 0) return basicEnemy;
        if (finalRandom == 1) return fastEnemy;
        if (finalRandom == 2) return rapidEnemy;
        if (finalRandom == 3) return tankEnemy;
        return berserkEnemy;
    }

    public void SetWave(int wave, float savedTimer)
    {
        currentWave = Mathf.Max(1, wave);
        waveTimer = savedTimer;
        waveActive = true;
        bossFightActive = false;
        nextSpawnTime = Time.time + 1f;
    }
}