using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Speed Settings")]
    public float baseCameraSpeed;
    public float playerMoveFactor;

    public float currentCameraSpeed;
    public float gameOverSlowdown = 0.1f;

    public static float screenWidth;
    public static float screenHeight;

    Camera cam;
    PlayerScript ps;

    Vector2 playerCameraVector;
    public static float overstepDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        screenHeight = cam.orthographicSize;
        screenWidth = screenHeight * cam.aspect;

        currentCameraSpeed = baseCameraSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.endGame)
        {
            currentCameraSpeed = Mathf.Max(0, currentCameraSpeed - gameOverSlowdown * Time.deltaTime);
        }
        else
        {
            playerCameraVector = ps.playerPos - new Vector2(transform.position.x, transform.position.y);

            float threshold = screenHeight - (3 * ps.playerHeight);

            if (playerCameraVector.y >= threshold)
            {
                overstepDistance = playerCameraVector.y - threshold;
                currentCameraSpeed = baseCameraSpeed + (overstepDistance * playerMoveFactor);
            }
            else
            {
                currentCameraSpeed = baseCameraSpeed;
            }
        }

        transform.position += new Vector3(0, currentCameraSpeed, 0) * Time.deltaTime;
    }

}