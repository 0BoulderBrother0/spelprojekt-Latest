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

    public void TriggerPlatformHelp()
    {
        if (leftTouching != rightTouching && rb.linearVelocityY <= PlayerScript.standStillThreshold)
        {
            if (leftTouching)
            {
                rb.AddForceX(-PlayerScript.towardsPlatformBoost * Time.deltaTime);
            }
            else
            {
                rb.AddForceX(PlayerScript.towardsPlatformBoost * Time.deltaTime);
            }
            rb.AddForceY(PlayerScript.platformBoost * Time.deltaTime);


            if (restrictVelocity == null)
                restrictVelocity = StartCoroutine(RestrictVelocity());
        }
    }


    IEnumerator RestrictVelocity()
    {
        Debug.Log("Started RestrictVelocity!");

        bool wasLeft = leftTouching;
        bool wasRight = rightTouching;

        while (true)
        {
            if (wasRight && rb.linearVelocityX < 0)
                rb.linearVelocityX = 0;

            if (wasLeft && rb.linearVelocityX > 0)
                rb.linearVelocityX = 0;

            if (GroundCheckScript.isOnGround && Mathf.Abs(rb.linearVelocityY) <= PlayerScript.standStillThreshold)
            {
                Debug.Log("Stopped RestrictVelocity");
                restrictVelocity = null;
                yield break;
            }

            yield return null;
        }
    }
}
