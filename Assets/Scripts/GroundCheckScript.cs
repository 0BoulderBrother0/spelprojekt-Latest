using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{

    public static bool isOnGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isOnGround = false;
            PlayerScript.touchedGround = false;
            Debug.Log($"isOnGround: {isOnGround}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isOnGround = true;
            Debug.Log($"isOnGround: {isOnGround}");
            
        }
    }
}
