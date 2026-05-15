using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public SpriteRenderer[] animations;
    PlatformManagerScript pms;
    Collider2D currentPlatformCollider;
    float xAxis;
    int nbrOfPlatforms;

    [Header("Player")]
    public float playerHeight;
    public float playerWidth;
    public Vector2 playerPos;
    public static bool hasJumped;
    public static float standStillThreshold = 0.1f;

    [Header("Platform Help")]
    public float platformHelpBoost;
    public float towardsPlatformHelpBoost;
    public static float platformBoost;
    public static float towardsPlatformBoost;
    public static bool insidePlatform;

    [Header("Speed")]
    public float moveSpeed;
    public float jumpHeight;
    public float airKoefficient = 0.1f;
    public float groundKoefficient = 0.1f;

    public static bool endGame;
    public static bool resumeGame;
    bool underScreen;
    public float SecondsBeforeDeath;
    Coroutine startEndGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pms = GameObject.FindGameObjectWithTag("PlatformManager").GetComponent<PlatformManagerScript>();

        playerHeight = sr.bounds.extents.y;
        playerWidth = sr.bounds.extents.x;

        platformBoost = platformHelpBoost;
        towardsPlatformBoost = towardsPlatformHelpBoost;
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        playerPos = transform.position;

        if (rb.linearVelocityX < -standStillThreshold)
        {
            sr.flipX = true;
        }
        else if (rb.linearVelocityX > standStillThreshold)
        {
            sr.flipX = false;
        }


        if (GroundCheckScript.isOnGround && Mathf.Abs(rb.linearVelocityY) <= standStillThreshold)
        {
            hasJumped = false;
            //touchedGround = true;
            //Debug.Log($"hasJumped: {hasJumped}");
            if (currentPlatformCollider != null)
            {
                nbrOfPlatforms++;
                GUIScript.tmp.text = $"Score: {nbrOfPlatforms}";

                pms.platformsColliders.Remove(currentPlatformCollider);
                currentPlatformCollider = null;
            }

            rb.linearVelocityX = rb.linearVelocityX * groundKoefficient * Time.deltaTime;

            sr.sprite = animations[0].sprite;

            if (xAxis < 0)
            {
                sr.flipX = true;
            }
            else if (xAxis > 0)
            {
                sr.flipX = false;
            }
        }


        if (Input.GetKey(KeyCode.Space) && !hasJumped)
        {
            sr.sprite = animations[1].sprite;
        }

        if (Input.GetKeyUp(KeyCode.Space) && !hasJumped)
        {
            hasJumped = true;
            Debug.Log($"hasJumped: {hasJumped}");

            rb.linearVelocityY = jumpHeight;
            rb.AddForceX(xAxis * moveSpeed, ForceMode2D.Impulse);

            sr.sprite = animations[2].sprite;
        }


        if (hasJumped)
        {
            rb.AddForceX(xAxis * moveSpeed * airKoefficient * Time.deltaTime, ForceMode2D.Force);
            //Debug.Log($"Force in air: {xAxis * moveSpeed * airKoefficient}");

            if (rb.linearVelocityY >= standStillThreshold)
            {
                sr.sprite = animations[2].sprite;
            }
            else
            {
                sr.sprite = animations[3].sprite;
            }
        }


        if (playerPos.y + playerHeight <= Camera.main.transform.position.y - CameraScript.screenHeight && !underScreen)
        {
            Debug.Log("Started EndGame");
            underScreen = true;
            if (startEndGame == null)
            {
                startEndGame = StartCoroutine(StartEndGame());
            }
            else
            {
                StopCoroutine(startEndGame);
                startEndGame = StartCoroutine(StartEndGame());
            }
        }

        else if (playerPos.y + playerHeight > Camera.main.transform.position.y - CameraScript.screenHeight && underScreen)
        {
            underScreen = false;
            Debug.Log("Stopping EndGame");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (pms.platformsColliders.Contains(collision.collider) && GroundCheckScript.isOnGround)
            {
                currentPlatformCollider = collision.collider;
            }
        }
    }

    void EndGame()
    {
        Destroy(gameObject);
    }

    IEnumerator StartEndGame()
    {
        endGame = true;

        yield return new WaitForSeconds(SecondsBeforeDeath);

        if (underScreen)
        {
            Debug.Log("Ended game.");
            EndGame();
        }
        else
        {
            resumeGame = true;
            endGame = false;
            Debug.Log("Stopped EndGame!");
        }
    }


}
