using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{

    public static bool isOnGround;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isOnGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            isOnGround = true;      
    }
}
