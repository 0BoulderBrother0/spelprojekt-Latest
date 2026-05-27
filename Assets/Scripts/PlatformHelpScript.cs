using System.Collections;
using UnityEngine;

public class PlatformHelpScript : MonoBehaviour
{
    Rigidbody2D rb;
    public bool leftTouching;
    public bool rightTouching;
    public static Coroutine restrictVelocity;
    public static PlatformHelpScript instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (restrictVelocity == null) return;
        if (PlayerScript.hasJumped) return;

        if (rightTouching && rb.linearVelocityX < 0)
            rb.linearVelocityX = 0;

        if (leftTouching && rb.linearVelocityX > 0)
            rb.linearVelocityX = 0;
    }

    public void TriggerPlatformHelp()
    {
        if (leftTouching != rightTouching && rb.linearVelocityY <= 0 && !PlayerScript.insidePlatform)
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
