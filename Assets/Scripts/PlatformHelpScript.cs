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
        if (leftTouching != rightTouching)
        {
            rb.AddForceY(PlayerScript.platformBoost * Time.deltaTime);
        }
    }
}
