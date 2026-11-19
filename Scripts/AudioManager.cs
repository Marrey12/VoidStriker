using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip playerShootSfx;
    [SerializeField] private AudioClip enemyDeathSfx;
    [SerializeField] private AudioClip playerHitSfx;
    [SerializeField] private AudioClip healPowerUpSfx;
    [SerializeField] private AudioClip speedPowerUpSfx;
    [SerializeField] private AudioClip rapidFirePowerUpSfx;
    [SerializeField] private AudioClip waveCompletedSfx;
    [SerializeField] private AudioClip bossAttackSfx;
    [SerializeField] private AudioClip gameOverSfx;
    [SerializeField] private AudioClip enemyShootSfx;
    [SerializeField] private AudioClip enemySpawnSfx;
    [SerializeField] private AudioClip berserkActivateSfx;

    [Header("SFX Volumes")]
    [SerializeField] private float enemyDeathVolume = 0.4f;
    [SerializeField] private float enemySpawnVolume = 0.3f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayPlayerShoot()
    {
        PlaySfx(playerShootSfx);
    }

    public void PlayEnemyDeath()
    {
        PlaySfx(enemyDeathSfx, enemyDeathVolume);
    }

    public void PlayPlayerHit()
    {
        PlaySfx(playerHitSfx);
    }

   

    public void PlayWaveCompleted()
    {
        PlaySfx(waveCompletedSfx);
    }

    public void PlayBossAttack()
    {
        PlaySfx(bossAttackSfx);
    }

    public void PlayGameOver()
    {
        PlaySfx(gameOverSfx);
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip);
    }
    public void PlayEnemyShoot()
    {
        PlaySfx(enemyShootSfx);
    }

    public void PlayEnemySpawn()
    {
        PlaySfx(enemySpawnSfx, enemySpawnVolume);
    }

    public void PlayBerserkActivate()
    {
        PlaySfx(berserkActivateSfx);
    }
    public void PlayHealPowerUp()
    {
        PlaySfx(healPowerUpSfx);
    }

    public void PlaySpeedPowerUp()
    {
        PlaySfx(speedPowerUpSfx);
    }

    public void PlayRapidFirePowerUp()
    {
        PlaySfx(rapidFirePowerUpSfx);
    }
    private void PlaySfx(AudioClip clip, float volume)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }
}
