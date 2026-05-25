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
    float[] currentPlatformDimensions;
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
    public float airSpeedupKoefficient;
    public float groundSlowdownKoefficient;

    [Header("End/Resume Game")]
    public static bool endGame;
    public float loseAppearTime;
    public static bool resumeGame;
    bool underScreen;
    public float SecondsBeforeDeath;
    Coroutine startEndGame;


    [Header("Powerup")]
    public Coroutine jumpPowerupActive;
    Coroutine fadeIconJump;
    public float jumpPowerupActiveTime;
    float jumpBoost = 1;
    public float jumpPowerupBoost;
    int jumpBonus = 0;
    public int jumpPowerupBonusScore = 1;

    public Coroutine invincibilityPowerupActive;
    Coroutine fadeIconInvincibility;
    public float invincibilityPowerupActiveTime;
    bool avoidScreenEdges;
    public float avoidScreenForceY;
    public float avoidScreenForceX;
    public float goThroughGroundDuration = 0.5f;
    public float goThroughGroundThreshold = 1;

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
            sr.flipX = true;
        else if (rb.linearVelocityX > standStillThreshold)
            sr.flipX = false;

        if (rb.linearVelocityY >= standStillThreshold)
            sr.sprite = animations[2].sprite;
        if (rb.linearVelocityY < standStillThreshold)
            sr.sprite = animations[3].sprite;


        if (GroundCheckScript.isOnGround && Mathf.Abs(rb.linearVelocityY) <= standStillThreshold)
        {
            hasJumped = false;
            //touchedGround = true;
            //Debug.Log($"hasJumped: {hasJumped}");
            if (currentPlatformCollider != null)
            {
                nbrOfPlatforms += 1 + jumpBonus;
                GUIScript.score.text = $"Score: {nbrOfPlatforms}";

                pms.platformsColliders.Remove(currentPlatformCollider);
                currentPlatformCollider = null;
            }

            sr.sprite = animations[0].sprite;

            if (xAxis < 0)
                sr.flipX = true;
            else if (xAxis > 0)
                sr.flipX = false;
        }

        if (Input.GetKey(KeyCode.Space) && !hasJumped)
            sr.sprite = animations[1].sprite;

        if (Input.GetKeyUp(KeyCode.Space) && !hasJumped)
        {
            hasJumped = true;
            //Debug.Log($"hasJumped: {hasJumped}");

            rb.linearVelocityY = jumpHeight * jumpBoost;
            rb.AddForceX(xAxis * moveSpeed, ForceMode2D.Impulse);

            sr.sprite = animations[2].sprite;
        }


        if (playerPos.y + playerHeight <= Camera.main.transform.position.y - CameraScript.screenHeight && !underScreen)
        {
            Debug.Log("Started EndGame");
            underScreen = true;
            if (startEndGame == null)
                startEndGame = StartCoroutine(StartEndGame());
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

    void FixedUpdate()
    {
        if (hasJumped)
            rb.AddForceX(xAxis * moveSpeed * airSpeedupKoefficient, ForceMode2D.Force);

        if (GroundCheckScript.isOnGround && Mathf.Abs(rb.linearVelocityY) <= standStillThreshold)
            rb.linearVelocityX = Mathf.Lerp(rb.linearVelocityX, 0, groundSlowdownKoefficient);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            currentPlatformDimensions = pms.FindObjectDimensions(collision.gameObject);
            if (pms.platformsColliders.Contains(collision.collider) && GroundCheckScript.isOnGround)
            {
                currentPlatformCollider = collision.collider;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            if (Mathf.Abs(collision.transform.position.x - playerPos.x) < currentPlatformDimensions[0] * 2 && hasJumped)
            {
                Debug.Log("Inside platform!");
                insidePlatform = true;
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpPowerup"))
        {
            Destroy(collision.gameObject);
            if (jumpPowerupActive == null)
                jumpPowerupActive = StartCoroutine(JumpPowerup());

            else
            {
                StopCoroutine(jumpPowerupActive);
                jumpPowerupActive = StartCoroutine(JumpPowerup());
            }
        }

        if (collision.CompareTag("InvincibilityPowerup"))
        {
            Destroy(collision.gameObject);
            if (invincibilityPowerupActive == null)
                invincibilityPowerupActive = StartCoroutine(InvincibilityPowerup());

            else
            {
                StopCoroutine(invincibilityPowerupActive);
                invincibilityPowerupActive = StartCoroutine(InvincibilityPowerup());
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Debug.Log("Outside platform!");
            insidePlatform = false;
        }
    }

    void EndGame()
    {
        Color cScore = GUIScript.score.color;
        Color cJumpPowerup = GUIScript.jumpPowerup.color;
        cScore.a = 0f;
        cJumpPowerup.a = 0f;
        GUIScript.score.color = cScore;
        GUIScript.jumpPowerup.color = cJumpPowerup;

        GUIScript.instance.StopAllCoroutines();

        GUIScript.instance.StartCoroutine(GUIScript.ShowText(GUIScript.lose, loseAppearTime));

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

    IEnumerator JumpPowerup()
    {
        Color c = GUIScript.jumpPowerup.color;
        c.a = 1f;
        GUIScript.jumpPowerup.color = c;

        if (fadeIconJump != null)
        {
            GUIScript.instance.StopCoroutine(fadeIconJump);
            fadeIconJump = GUIScript.instance.StartCoroutine(GUIScript.FadeIcon(GUIScript.jumpPowerup, jumpPowerupActiveTime));
        }
        else
            fadeIconJump = GUIScript.instance.StartCoroutine(GUIScript.FadeIcon(GUIScript.jumpPowerup, jumpPowerupActiveTime));


        jumpBoost = jumpPowerupBoost;
        jumpBonus = jumpPowerupBonusScore;

        Debug.Log("Activated jump powerup!");

        yield return new WaitForSeconds(jumpPowerupActiveTime);

        jumpPowerupActive = null;
        jumpBoost = 1;
        jumpBonus = 0;

        Debug.Log("Disabled jump powerup");
    }

    IEnumerator InvincibilityPowerup()
    {
        Color c = GUIScript.invincibilityPowerup.color;
        c.a = 1f;
        GUIScript.invincibilityPowerup.color = c;

        if (fadeIconInvincibility != null)
        {
            GUIScript.instance.StopCoroutine(fadeIconInvincibility);
            fadeIconInvincibility = GUIScript.instance.StartCoroutine(GUIScript.FadeIcon(GUIScript.invincibilityPowerup, invincibilityPowerupActiveTime));
        }
        else
        {
            fadeIconInvincibility = GUIScript.instance.StartCoroutine(GUIScript.FadeIcon(GUIScript.invincibilityPowerup, invincibilityPowerupActiveTime));
        }



        avoidScreenEdges = true;
        StartCoroutine(AvoidScreenEdges());

        Debug.Log("Activated invincibility powerup!");

        yield return new WaitForSeconds(invincibilityPowerupActiveTime);

        avoidScreenEdges = false;

        Debug.Log("Disabled invincibility powerup");
    }

    IEnumerator AvoidScreenEdges()
    {
        while (avoidScreenEdges)
        {
            Vector2 camPos = Camera.main.transform.position;


            if (Mathf.Abs(playerPos.x) + playerWidth + goThroughGroundThreshold >= camPos.x + CameraScript.screenWidth || playerPos.y - playerHeight - goThroughGroundThreshold <= camPos.y - CameraScript.screenHeight)
                StartCoroutine(TemporarilyIgnoreGround(goThroughGroundDuration));

            if (Mathf.Abs(playerPos.x) + playerWidth >= camPos.x + CameraScript.screenWidth)
            {
                if (PlatformHelpScript.restrictVelocity != null)
                    StopCoroutine(PlatformHelpScript.restrictVelocity);


                if (playerPos.x < 0)
                    rb.AddForceX(avoidScreenForceX, ForceMode2D.Force);

                else
                    rb.AddForceX(-avoidScreenForceX, ForceMode2D.Force);
            }


            if (playerPos.y - playerHeight <= camPos.y - CameraScript.screenHeight)
            {
                if (PlatformHelpScript.restrictVelocity != null)
                    StopCoroutine(PlatformHelpScript.restrictVelocity);

                rb.AddForceY(avoidScreenForceY, ForceMode2D.Force);
            }



            yield return null;
        }
        yield break;
    }

    IEnumerator TemporarilyIgnoreGround(float duration)
    {
        int playerLayer = gameObject.layer;
        int groundLayer = LayerMask.NameToLayer("Ground");

        Color c = sr.color;
        c.a = 0.7f;
        sr.color = c;


        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);

        Debug.Log("Going through ground!");

        yield return new WaitForSeconds(duration);

        c.a = 1f;
        sr.color = c;


        Debug.Log("Stopped going through ground");

        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
    }

}
