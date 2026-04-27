using UnityEngine;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    float xAxis;
    bool hasJumped;

    public float moveSpeed;
    public float jumpHeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");


        rb.linearVelocityX = xAxis * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && GroundCheckScript.isOnGround && hasJumped == false)
        {
            rb.linearVelocityY = jumpHeight;
            hasJumped = true;
        }

        if (GroundCheckScript.isOnGround == false)
        {
            hasJumped = false;
        }
    }


}
