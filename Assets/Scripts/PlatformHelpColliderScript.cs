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
            if (gameObject.CompareTag("PlatformHelpColliderRight"))
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
            if (gameObject.CompareTag("PlatformHelpColliderRight"))
                phs.rightTouching = false;
        }
    }
}
