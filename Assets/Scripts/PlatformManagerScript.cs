using System.Collections;
using UnityEngine;

public class PlatformManagerScript : MonoBehaviour
{
    public GameObject platformObject;
    public float interval;


    SpriteRenderer[] srArray;
    public float platformHeight;
    public float platformWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        srArray = platformObject.GetComponentsInChildren<SpriteRenderer>();

        // Start with the first sprite's bounds, then expand to include the rest
        Bounds totalBounds = srArray[0].bounds;
        foreach (SpriteRenderer sr in srArray)
        {
            totalBounds.Encapsulate(sr.bounds);
        }

        platformWidth = totalBounds.extents.x;
        platformHeight = totalBounds.extents.y;

        StartCoroutine(SpawnPlatforms());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnPlatforms()
    {
        while (true)
        {
            float platformPositionX = Random.Range(-CameraScript.screenWidth + platformWidth, CameraScript.screenWidth - platformWidth);
            Debug.Log(platformPositionX);

            Instantiate(platformObject, new Vector2(platformPositionX, CameraScript.screenHeight + platformHeight + Camera.main.transform.position.y), Quaternion.identity);

            yield return new WaitForSeconds(interval);
        }
    }
}
