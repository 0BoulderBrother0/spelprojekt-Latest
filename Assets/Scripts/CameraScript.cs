using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float cameraSpeed;

    public static float screenWidth;
    public static float screenHeight;

    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;

        screenHeight = screenWidth * cam.aspect;
        screenWidth = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, cameraSpeed, 0) * Time.deltaTime;
    }
}
