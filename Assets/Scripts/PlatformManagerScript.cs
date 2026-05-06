using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManagerScript : MonoBehaviour
{
    public GameObject platformObject;
    public List<Collider2D> platformsColliders;
    bool spawnedPlatform;

    SpriteRenderer[] srArray;
    public float platformHeight;
    public float platformWidth;

    Camera cam;

    void Start()
    {
        srArray = platformObject.GetComponentsInChildren<SpriteRenderer>();
        cam = Camera.main;

        Bounds totalBounds = srArray[0].bounds;
        foreach (SpriteRenderer sr in srArray)
        {
            totalBounds.Encapsulate(sr.bounds);
        }

        platformWidth = totalBounds.extents.x;
        platformHeight = totalBounds.extents.y;

        //StartCoroutine(SpawnPlatforms());
    }

    void Update()
    {
        if (Mathf.Round(cam.transform.position.y) % 4 == 0 && !spawnedPlatform)
        {
            float platformPositionX = Random.Range(-CameraScript.screenWidth + platformWidth, CameraScript.screenWidth - platformWidth);

            GameObject newPlatform = Instantiate(platformObject, new Vector2(platformPositionX, CameraScript.screenHeight + platformHeight + Camera.main.transform.position.y), Quaternion.identity);

            platformsColliders.Add(newPlatform.GetComponent<Collider2D>());

            spawnedPlatform = true;
        }
        else if (Mathf.Round(cam.transform.position.y) % 4 != 0)
        {
            spawnedPlatform = false;
        }
    }
}