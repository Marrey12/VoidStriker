using UnityEngine;

public class BerserkEnemy : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float berserkSpeed = 9f;

    private bool isBerserk = false;
    private Transform player;

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
        if (!isBerserk || player == null) return;

        Vector2 direction =
            player.position - transform.position;

        transform.Translate(
            direction.normalized * berserkSpeed * Time.deltaTime,
            Space.World
        );
    }

    public void ActivateBerserk()
    {
        isBerserk = true;
        AudioManager.Instance?.PlayBerserkActivate();
    }
}
