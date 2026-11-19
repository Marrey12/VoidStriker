using System.IO;
using TMPro;
using UnityEngine;

[System.Serializable]
public class AchievementData
{
    public bool sniperUnlocked;
    public bool unstoppableUnlocked;
    public bool lastHopeUnlocked;
    public bool oneShotUnlocked;
    public bool asteroidDanceUnlocked;
}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private TextMeshProUGUI achievementText;

    private AchievementData data;

    private int consecutiveHits = 0;
    private int killsWithoutDamage = 0;
    private float timeWithoutDamage = 0f;

    private static string GetPath()
    {
        return Application.persistentDataPath +
               "/achievements_player_" +
               PlayerProfileManager.CurrentPlayerId +
               ".json";
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        data = LoadAchievements();

        if (achievementPanel != null)
        {
            achievementPanel.SetActive(false);
        }
    }

    private void Update()
    {
        timeWithoutDamage += Time.deltaTime;

        if (!data.asteroidDanceUnlocked && timeWithoutDamage >= 60f)
        {
            data.asteroidDanceUnlocked = true;
            UnlockAchievement("Taniec wśród asteroid");
        }
    }

    public void RegisterHit()
    {
        consecutiveHits++;

        if (!data.sniperUnlocked && consecutiveHits >= 10)
        {
            data.sniperUnlocked = true;
            UnlockAchievement("Kosmiczny snajper");
        }
    }

    public void RegisterMiss()
    {
        consecutiveHits = 0;
    }

    public void RegisterEnemyKilled()
    {
        killsWithoutDamage++;

        if (!data.unstoppableUnlocked && killsWithoutDamage >= 25)
        {
            data.unstoppableUnlocked = true;
            UnlockAchievement("Nie do zatrzymania");
        }
    }

    public void RegisterPlayerDamage()
    {
        killsWithoutDamage = 0;
        timeWithoutDamage = 0f;
    }

    public void RegisterBossKilledLowHp(float hpPercent)
    {
        if (!data.lastHopeUnlocked && hpPercent <= 1f)
        {
            data.lastHopeUnlocked = true;
            UnlockAchievement("Ostatnia nadzieja galaktyki");
        }
    }

    public void RegisterOneShotKill(int killCount)
    {
        if (!data.oneShotUnlocked && killCount >= 3)
        {
            data.oneShotUnlocked = true;
            UnlockAchievement("Jednym strzałem");
        }
    }

    private void UnlockAchievement(string achievementName)
    {
        SaveAchievements(data);

        if (achievementPanel != null)
        {
            achievementPanel.SetActive(true);
        }

        if (achievementText != null)
        {
            achievementText.text =
                "ACHIEVEMENT UNLOCKED\n" +
                achievementName;
        }

        CancelInvoke(nameof(HideAchievement));
        Invoke(nameof(HideAchievement), 3f);
    }

    private void HideAchievement()
    {
        if (achievementPanel != null)
        {
            achievementPanel.SetActive(false);
        }
    }

    public static AchievementData LoadAchievementsForPlayer(int playerId)
    {
        string path =
            Application.persistentDataPath +
            "/achievements_player_" +
            playerId +
            ".json";

        if (!File.Exists(path))
        {
            return new AchievementData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<AchievementData>(json);
    }

    private static AchievementData LoadAchievements()
    {
        if (!File.Exists(GetPath()))
        {
            return new AchievementData();
        }

        string json = File.ReadAllText(GetPath());
        return JsonUtility.FromJson<AchievementData>(json);
    }

    private static void SaveAchievements(AchievementData dataToSave)
    {
        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(GetPath(), json);
    }
}
