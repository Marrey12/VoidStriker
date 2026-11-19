using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerText;

    [SerializeField] private GameObject highScoresPanel;
    [SerializeField] private TextMeshProUGUI highScoresText;

    [SerializeField] private GameObject achievementsPanel;
    [SerializeField] private TextMeshProUGUI achievementsText;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }

        UpdatePlayerText();

        if (highScoresPanel != null)
            highScoresPanel.SetActive(false);

        if (achievementsPanel != null)
            achievementsPanel.SetActive(false);
    }

    public void StartGame()
    {
        GameDataHolder.LoadedData = null;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGame()
    {
        SaveData data = SaveManager.LoadGame();

        if (data != null)
        {
            GameDataHolder.LoadedData = data;
            SceneManager.LoadScene("GameScene");
        }
    }

    public void ChangePlayer()
    {
        int nextPlayer = PlayerProfileManager.CurrentPlayerId + 1;

        if (nextPlayer > 4)
            nextPlayer = 1;

        PlayerProfileManager.SetPlayer(nextPlayer);

        UpdatePlayerText();
    }

    private void UpdatePlayerText()
    {
        if (playerText != null)
        {
            playerText.text =
                "PLAYER: " +
                PlayerProfileManager.CurrentPlayerName;
        }
    }

    public void ShowHighScores()
    {
        if (highScoresPanel != null)
            highScoresPanel.SetActive(true);

        HighScoreData data =
            HighScoreManager.LoadScores(
                PlayerProfileManager.CurrentPlayerId
            );

        string text =
            "HIGH SCORES - " +
            PlayerProfileManager.CurrentPlayerName +
            "\n\n";

        for (int i = 0; i < 5; i++)
        {
            if (i < data.scores.Count)
                text += (i + 1) + ". " + data.scores[i] + "\n";
            else
                text += (i + 1) + ". ---\n";
        }

        if (highScoresText != null)
            highScoresText.text = text;
    }

    public void HideHighScores()
    {
        if (highScoresPanel != null)
            highScoresPanel.SetActive(false);
    }

    public void ShowAchievements()
    {
        if (achievementsPanel != null)
            achievementsPanel.SetActive(true);

        AchievementData data =
            AchievementManager.LoadAchievementsForPlayer(
                PlayerProfileManager.CurrentPlayerId
            );

        string text =
            "ACHIEVEMENTS - " +
            PlayerProfileManager.CurrentPlayerName +
            "\n\n";

        text += GetAchievementLine(
            data.sniperUnlocked,
            "Kosmiczny snajper",
            "Traf 10 przeciwników z rzędu bez chybienia."
        );

        text += GetAchievementLine(
            data.unstoppableUnlocked,
            "Nie do zatrzymania",
            "Zniszcz 25 przeciwników bez otrzymania obrażeń."
        );

        text += GetAchievementLine(
            data.lastHopeUnlocked,
            "Ostatnia nadzieja galaktyki",
            "Pokonaj bossa posiadając tylko 1% HP."
        );

        text += GetAchievementLine(
            data.oneShotUnlocked,
            "Jednym strzałem",
            "Zniszcz 3 przeciwników jednym Area Shotem."
        );

        text += GetAchievementLine(
            data.asteroidDanceUnlocked,
            "Taniec wśród asteroid",
            "Przetrwaj 60 sekund bez otrzymania obrażeń."
        );

        if (achievementsText != null)
            achievementsText.text = text;
    }

    private string GetAchievementLine(
        bool unlocked,
        string name,
        string description)
    {
        string status = unlocked ? "✓" : "X";

        return
            status + " " + name + "\n" +
            "   " + description + "\n\n";
    }

    public void HideAchievements()
    {
        if (achievementsPanel != null)
            achievementsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
