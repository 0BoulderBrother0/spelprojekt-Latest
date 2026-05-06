using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManagerScript : MonoBehaviour
{
    public GameObject platformObject;
    public int maxNbrPlatformsPerBatch = 4;
    public float distanceBetween = 4;
    GameObject newPlatform;
    public List<Collider2D> platformsColliders;
    bool spawnedPlatforms;

    Camera cam;


    SpriteRenderer[] srArray;
    float platformWidth;
    float platformHeight;


    int nbrTries;
    public int triesSpawningPlatform = 3;
    bool skipPlatform;


    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (Mathf.Round(cam.transform.position.y) % distanceBetween == 0 && !spawnedPlatforms)
        {
            int nbrPlatforms = Random.Range(1, maxNbrPlatformsPerBatch);
            List<(float position, float width)> spawnedPositions = new List<(float, float)>();

            for (int i = 0; i < nbrPlatforms; i++)
            {
                float platformPositionX;
                bool overlapping;
                nbrTries = 0;
                skipPlatform = false;

                if (platformWidth == 0 || platformHeight == 0)
                {
                    srArray = platformObject.GetComponentsInChildren<SpriteRenderer>();

                    Bounds totalBounds = srArray[0].bounds;
                    foreach (SpriteRenderer sr in srArray)
                    {
                        totalBounds.Encapsulate(sr.bounds);
                    }

                    platformWidth = totalBounds.extents.x;
                    platformHeight = totalBounds.extents.y;
                }

                do
                {
                    overlapping = false;
                    platformPositionX = Random.Range(-CameraScript.screenWidth + platformWidth, CameraScript.screenWidth - platformWidth);

                    foreach (var (pos, width) in spawnedPositions)
                    {
                        if (Mathf.Abs(platformPositionX - pos) < width * 2)
                        {
                            overlapping = true;
                            break;
                        }
                    }

                    nbrTries++;
                    if (nbrTries >= triesSpawningPlatform)
                    {
                        skipPlatform = true;
                        break;
                    }
                } while (overlapping);

                if (!skipPlatform)
                {
                    spawnedPositions.Add((platformPositionX, platformWidth));
                    newPlatform = Instantiate(platformObject, new Vector2(platformPositionX, CameraScript.screenHeight + platformHeight + cam.transform.position.y), Quaternion.identity);

                    PlatformScript platformScript = newPlatform.GetComponent<PlatformScript>();
                    platformWidth = platformScript.platformWidth;
                    platformHeight = platformScript.platformHeight;

                    platformsColliders.Add(newPlatform.GetComponent<Collider2D>());
                }
            }

            spawnedPlatforms = true;
        }
        else if (Mathf.Round(cam.transform.position.y) % distanceBetween != 0)
        {
            spawnedPlatforms = false;
        }
    }
}