using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(0, cam.transform.position.y, 0);
    }
}
