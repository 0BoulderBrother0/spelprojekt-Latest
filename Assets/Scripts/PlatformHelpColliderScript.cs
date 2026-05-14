using Unity.VisualScripting;
using UnityEngine;

public class PlatformHelpColliderScript : MonoBehaviour
{
    PlatformHelpScript phs;

    void Start()
    {
        phs = GetComponentInParent<PlatformHelpScript>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (gameObject.CompareTag("PlatformHelpColliderLeft"))
                phs.leftTouching = true;
            else if (gameObject.CompareTag("PlatformHelpColliderRight"))
                phs.rightTouching = true;

            phs.TriggerPlatformHelp();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (gameObject.CompareTag("PlatformHelpColliderLeft"))
                phs.leftTouching = false;
            else if (gameObject.CompareTag("PlatformHelpColliderRight"))
                phs.rightTouching = false;
        }
    }
}
