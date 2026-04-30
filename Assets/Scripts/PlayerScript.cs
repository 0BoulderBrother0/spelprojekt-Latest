using UnityEngine;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    float xAxis;
    public static bool hasJumped;
    public static bool touchedGround;

    public static float velocityThreshold = -1;

    [Header("speed")]
    public float moveSpeed;
    public float jumpHeight;
    public float airKoefficient = 0.1f;
    public float groundKoefficient = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if (GroundCheckScript.isOnGround && rb.linearVelocityY < velocityThreshold && !touchedGround)
        {
            hasJumped = false;
            touchedGround = true;
            Debug.Log($"hasJumped: {hasJumped}");

            rb.linearVelocityX = rb.linearVelocityX * groundKoefficient;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
        {
            hasJumped = true;
            Debug.Log($"hasJumped: {hasJumped}");

            rb.linearVelocityY = jumpHeight;
            rb.AddForceX(xAxis * moveSpeed, ForceMode2D.Impulse);
            Debug.Log($"Force before air: {xAxis * moveSpeed}");
        }

        if (hasJumped)
        {
            rb.AddForceX(xAxis * moveSpeed * airKoefficient, ForceMode2D.Force);
            Debug.Log($"Force in air: {xAxis * moveSpeed * airKoefficient}");
        }
    }


}
