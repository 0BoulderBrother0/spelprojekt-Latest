using UnityEngine;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviour
{
    //Tjenare
    Rigidbody2D rb;
    SpriteRenderer sr;
    PlatformManagerScript pms;
    Collider2D currentPlatformCollider;
    float xAxis;
    float playerHeight;
    float playerWidth;
    int nbrOfPlatforms;

    public static bool hasJumped;


    public float SecondsDisabledIsOnGround = 1;
    float secondsSinceJump;

    [Header("Platform Help")]
    public float platformHelpDistance;
    public float platformDistanceBuffer;
    public float platformHelpBoost = 3;

    [Header("Speed")]
    public float moveSpeed;
    public float jumpHeight;
    public float airKoefficient = 0.1f;
    public float groundKoefficient = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pms = GameObject.FindGameObjectWithTag("PlatformManager").GetComponent<PlatformManagerScript>();

        playerHeight = sr.bounds.extents.y;
        playerWidth = sr.bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        secondsSinceJump += Time.deltaTime;
        

        if (GroundCheckScript.isOnGround && SecondsDisabledIsOnGround <= secondsSinceJump)
        {
            hasJumped = false;
            //touchedGround = true;
            //Debug.Log($"hasJumped: {hasJumped}");
            if(currentPlatformCollider != null)
            {
                nbrOfPlatforms++;
                GUIScript.tmp.text = $"Score: {nbrOfPlatforms}";

                currentPlatformCollider = null;
            }

            rb.linearVelocityX = rb.linearVelocityX * groundKoefficient * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
        {
            secondsSinceJump = 0;
            hasJumped = true;
            Debug.Log($"hasJumped: {hasJumped}");

            rb.linearVelocityY = jumpHeight;
            rb.AddForceX(xAxis * moveSpeed, ForceMode2D.Impulse);
            //Debug.Log($"Force before air: {xAxis * moveSpeed}");
        }

        if (hasJumped)
        {
            rb.AddForceX(xAxis * moveSpeed * airKoefficient * Time.deltaTime, ForceMode2D.Force);
            //Debug.Log($"Force in air: {xAxis * moveSpeed * airKoefficient}");
        }
        
    }

    void OnCollisionStay2D(Collision2D collision)
{
    if (collision.collider.CompareTag("Ground"))
    {
        if (pms.platformsColliders.Contains(collision.collider))
        {
                currentPlatformCollider = collision.collider;
                pms.platformsColliders.Remove(collision.collider);
        }
        

        Vector2 playerPos = transform.position;
        Vector2 platformPos = collision.collider.transform.position;

        float platformTop = collision.collider.bounds.max.y;
        float platformWidth = collision.collider.bounds.extents.x;


        if (platformTop - (playerPos.y - playerHeight) <= platformHelpDistance && playerPos.y - playerHeight + platformDistanceBuffer < platformTop && (platformPos.x + platformWidth <= playerPos.x - playerWidth || platformPos.x - platformWidth >= playerPos.x + playerWidth))
        {
            Debug.Log("Triggered platform help!");
            //transform.position = new Vector2(playerPos.x, platformTop + playerHeight + platformDistanceBuffer);
            rb.AddForceY(platformTop + playerHeight + platformHelpBoost - playerPos.y);
        }
    }
}


}
