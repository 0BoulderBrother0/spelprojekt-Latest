using System.Collections;
using UnityEngine;

public class PlatformHelpScript : MonoBehaviour
{
    Rigidbody2D rb;
    public bool leftTouching;
    public bool rightTouching;
    public static Coroutine restrictVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (restrictVelocity == null) return;

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
                rb.AddForceX(-PlayerScript.towardsPlatformBoost * Time.deltaTime);
            else
                rb.AddForceX(PlayerScript.towardsPlatformBoost * Time.deltaTime);


            rb.AddForceY(PlayerScript.platformBoost * Time.deltaTime);


            if (restrictVelocity == null)
                restrictVelocity = StartCoroutine(RestrictVelocity());
        }
    }


    public IEnumerator RestrictVelocity()
    {
        while (true)
        {
            if (GroundCheckScript.isOnGround && Mathf.Abs(rb.linearVelocityY) <= PlayerScript.standStillThreshold)
            {
                restrictVelocity = null;
                yield break;
            }
            yield return null;
        }
    }
}
