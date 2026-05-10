using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    PlayerScript ps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(0, ps.transform.position.y, 0);
    }
}
