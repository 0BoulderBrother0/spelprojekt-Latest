using System.Collections;
using UnityEngine;

public class PlatformHelpScript : MonoBehaviour
{
    Rigidbody2D rb;
    public bool leftTouching;
    public bool rightTouching;
    public static Coroutine restrictVelocity;
    public static PlatformHelpScript instance;

    float stuckTimer;
    public float timeBeforeStuckBoost = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        if (leftTouching && rightTouching && Mathf.Abs(rb.linearVelocityY) <= PlayerScript.standStillThreshold)
        {
            stuckTimer += 1 * Time.deltaTime;
            if (stuckTimer >= timeBeforeStuckBoost / CameraScript.progressionKoefficient)
            {
                rb.AddForceY(PlayerScript.platformBoost, ForceMode2D.Impulse);
                stuckTimer = 0;
            }
        }
    }

    void FixedUpdate()
    {
        if (restrictVelocity == null || PlayerScript.ignoreGround) return;

        if (rightTouching && rb.linearVelocityX < 0)
            rb.linearVelocityX = 0;

        if (leftTouching && rb.linearVelocityX > 0)
            rb.linearVelocityX = 0;
    }

    public void TriggerPlatformHelp()
    {
        if (leftTouching != rightTouching && rb.linearVelocityY <= 0 && !PlayerScript.insidePlatform && !PlayerScript.ignoreGround)
        {
            if (leftTouching)
                rb.AddForceX(-PlayerScript.towardsPlatformBoost);
            else
                rb.AddForceX(PlayerScript.towardsPlatformBoost);


            rb.AddForceY(PlayerScript.platformBoost);


            if (restrictVelocity == null)
                restrictVelocity = StartCoroutine(RestrictVelocity());

            Debug.Log("Triggered platform help!");
        }
    }


    public IEnumerator RestrictVelocity()
    {
        while (true)
        {
            if (GroundCheckScript.isOnGround && Mathf.Abs(rb.linearVelocityY) <= PlayerScript.standStillThreshold && !PlayerScript.ignoreGround)
            {
                restrictVelocity = null;
                yield break;
            }
            yield return null;
        }
    }
}
