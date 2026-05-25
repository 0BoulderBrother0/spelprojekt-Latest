using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManagerScript : MonoBehaviour
{
    public GameObject platformObject;
    public int maxNbrPlatformsPerBatch = 4;
    public float distanceBetween = 4;
    public List<Collider2D> platformsColliders;
    bool hasSpawnedPlatforms;

    Camera cam;
    PlatformScript platformScript;

    SpriteRenderer[] srArray;
    float originalPlatformWidth;
    float originalPlatformHeight;
    public static float platformMaxScale = 2f;


    int nbrTries;
    public int triesSpawningPlatform = 3;
    bool skipPlatform;
    bool isReady;

    [Header("Powerup")]
    public GameObject[] powerupObjects;
    public float lengthAbovePlatform = 1;

    IEnumerator Start()
    {
        cam = Camera.main;


        yield return null;


        float[] originalPlatformDimensions = FindObjectDimensions(platformObject);

        originalPlatformWidth = originalPlatformDimensions[0];
        originalPlatformHeight = originalPlatformDimensions[1];

         isReady = true;
    }

    void Update()
    {
        if (isReady)
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

                    do
                    {
                        overlapping = false;
                        newPlatformPositionX = Random.Range(-CameraScript.screenWidth + originalPlatformWidth * platformMaxScale, CameraScript.screenWidth - originalPlatformWidth * platformMaxScale);

                        foreach (GameObject platform in spawnedPlatforms)
                        {
                            if (Mathf.Abs(newPlatformPositionX - platform.transform.position.x) < originalPlatformWidth * 2 * platformMaxScale)
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
                        GameObject newPlatform = Instantiate(platformObject, new Vector2(newPlatformPositionX, CameraScript.screenHeight + originalPlatformHeight + cam.transform.position.y), Quaternion.identity);
                        spawnedPlatforms.Add(newPlatform);

                        SpawnPowerup(newPlatform);

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

    float[] FindObjectDimensions(GameObject gameObject)
    {
        SpriteRenderer[] srArray = gameObject.GetComponentsInChildren<SpriteRenderer>();

        Bounds totalBounds = srArray[0].bounds;
        foreach (SpriteRenderer sr in srArray)
        {
            totalBounds.Encapsulate(sr.bounds);
        }

        return new float[2] { totalBounds.extents.x, totalBounds.extents.y };
    }

    void SpawnPowerup(GameObject platformObject)
    {
        float[] platformDimensions = FindObjectDimensions(platformObject);

        float spawnPositionX = platformObject.transform.position.x + Random.Range(-platformDimensions[0], platformDimensions[0]);
        float spawnPositionY = platformObject.transform.position.y + platformDimensions[1] + lengthAbovePlatform;

        int powerupObjectIndex = Random.Range(0, powerupObjects.Length);
        Instantiate(powerupObjects[powerupObjectIndex], new Vector2(spawnPositionX, spawnPositionY), Quaternion.identity);
    }
}

