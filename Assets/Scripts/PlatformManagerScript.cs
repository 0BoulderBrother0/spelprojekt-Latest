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
    bool hasSpawnedPlatforms;

    Camera cam;


    SpriteRenderer[] srArray;
    float platformWidth;
    float platformHeight;
    public static float platformMaxScale = 2f;


    int nbrTries;
    public int triesSpawningPlatform = 3;
    bool skipPlatform;


    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (Mathf.Round(cam.transform.position.y) % distanceBetween == 0 && !hasSpawnedPlatforms)
        {
            int nbrPlatforms = Random.Range(1, maxNbrPlatformsPerBatch);
            List<GameObject> spawnedPlatforms = new List<GameObject>();

            for (int i = 0; i < nbrPlatforms; i++)
            {
                float newPlatformPositionX;
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
                    newPlatformPositionX = Random.Range(-CameraScript.screenWidth + platformWidth * platformMaxScale, CameraScript.screenWidth - platformWidth * platformMaxScale);

                    foreach (GameObject platform in spawnedPlatforms)
                    {
                        if (Mathf.Abs(newPlatformPositionX - platform.transform.position.x) < platformWidth * 2 * platformMaxScale)
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
                    newPlatform = Instantiate(platformObject, new Vector2(newPlatformPositionX, CameraScript.screenHeight + platformHeight + cam.transform.position.y), Quaternion.identity);
                    spawnedPlatforms.Add(newPlatform);

                    PlatformScript platformScript = newPlatform.GetComponent<PlatformScript>();
                    platformWidth = platformScript.platformWidth;
                    platformHeight = platformScript.platformHeight;

                    platformsColliders.Add(newPlatform.GetComponent<Collider2D>());
                }
            }

            hasSpawnedPlatforms = true;
        }
        else if (Mathf.Round(cam.transform.position.y) % distanceBetween != 0)
        {
            hasSpawnedPlatforms = false;
        }
    }
}