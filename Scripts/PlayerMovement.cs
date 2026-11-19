using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private float minX, maxX, minY, maxY;

    private float currentMoveSpeed;
    private float speedBoostTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentMoveSpeed = moveSpeed;

        Camera cam = Camera.main;

        float vertExtent = cam.orthographicSize;
        float horExtent = vertExtent * Screen.width / Screen.height;

        minX = -horExtent + 0.5f;
        maxX = horExtent - 0.5f;
        minY = -vertExtent + 0.5f;
        maxY = vertExtent - 0.5f;
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        UpdateSpeedBoost();
    }

    private void FixedUpdate()
    {
        Vector2 newPosition =
            rb.position +
            movement * currentMoveSpeed * Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rb.MovePosition(newPosition);
    }

    private void UpdateSpeedBoost()
    {
        if (speedBoostTimer > 0f)
        {
            speedBoostTimer -= Time.deltaTime;

            if (speedBoostTimer <= 0f)
            {
                currentMoveSpeed = moveSpeed;
            }
        }
    }

    public void ActivateSpeedBoost(
        float boostMultiplier,
        float duration)
    {
        currentMoveSpeed =
            moveSpeed * boostMultiplier;

        speedBoostTimer = duration;
    }
}