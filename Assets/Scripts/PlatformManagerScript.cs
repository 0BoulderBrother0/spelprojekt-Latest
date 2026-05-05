using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManagerScript : MonoBehaviour
{
    public GameObject platformObject;
    public List<Collider2D> platformsColliders;
    public float interval;
    int i;


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

            GameObject newPlatform = Instantiate(platformObject, new Vector2(platformPositionX, CameraScript.screenHeight + platformHeight + Camera.main.transform.position.y), Quaternion.identity);

            platformsColliders.Add(newPlatform.GetComponent<Collider2D>());

            yield return new WaitForSeconds(interval);
        }
    }
}
