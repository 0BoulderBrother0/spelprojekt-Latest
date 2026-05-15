using UnityEngine;

public class PlatformHelpScript : MonoBehaviour
{
    Rigidbody2D rb;
    public bool leftTouching;
    public bool rightTouching;

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
                if (rb.linearVelocityX < -PlayerScript.standStillThreshold)
                {
                    rb.linearVelocityX = -PlayerScript.standStillThreshold;
                }
            }
            else
            {
                rb.AddForceX(PlayerScript.towardsPlatformBoost * Time.deltaTime);
                if (rb.linearVelocityX > PlayerScript.standStillThreshold)
                {
                    rb.linearVelocityX = PlayerScript.standStillThreshold;
                }
            }
            rb.AddForceY(PlayerScript.platformBoost * Time.deltaTime);
        }
    }
}
