using TMPro;
using UnityEngine;

public class WeaponCooldownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI doubleShotText;
    [SerializeField] private TextMeshProUGUI spreadShotText;
    [SerializeField] private TextMeshProUGUI areaShotText;

    private PlayerShooting playerShooting;

    private void Start()
    {
        playerShooting =
            FindFirstObjectByType<PlayerShooting>();
    }

    private void Update()
    {
        if (playerShooting == null)
            return;

        UpdateCooldown(
            doubleShotText,
            "DOUBLE [2]",
            playerShooting.DoubleCooldown
        );

        UpdateCooldown(
            spreadShotText,
            "SPREAD [3]",
            playerShooting.SpreadCooldown
        );

        UpdateCooldown(
            areaShotText,
            "AREA [4]",
            playerShooting.AreaCooldown
        );
    }

    private void UpdateCooldown(
        TextMeshProUGUI text,
        string label,
        float cooldown)
    {
        if (cooldown > 0f)
        {
            text.text =
                label +
                "\n" +
                Mathf.CeilToInt(cooldown);
        }
        else
        {
            text.text =
                label +
                "\nREADY";
        }
    }
}
