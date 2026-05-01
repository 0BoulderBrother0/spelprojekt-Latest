using System.Collections;
using UnityEngine;

public class PlatformManagerScript : MonoBehaviour
{
    public GameObject platformObject;
    public float interval;

    
    SpriteRenderer[] srArray;
    float platformHeight;
    float platformWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        srArray = platformObject.GetComponentsInChildren<SpriteRenderer>();

        // Tar största längden hittad mellan alla segment
        foreach (SpriteRenderer sr in srArray)
        {
            platformWidth += sr.bounds.extents.x;

            if (sr.bounds.extents.y > platformHeight)
            {
                platformHeight = sr.bounds.extents.y;
            }
        }


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
