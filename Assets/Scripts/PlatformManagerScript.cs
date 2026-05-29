using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManagerScript : MonoBehaviour
{
    public GameObject platformObject;
    public int maxNbrPlatformsPerBatch = 4;
    public float distanceBetween = 4;
    public List<Collider2D> platformsColliders;

    Camera cam;

    float originalPlatformWidth;
    float originalPlatformHeight;
    public static float platformMaxScale = 2f;
    public float safetyDistance = 0.05f;


    int nbrTries;
    float lastSpawnY;
    public int triesSpawningPlatform = 3;
    bool skipPlatform;
    bool isReady;

    [Header("Powerup")]
    public GameObject[] powerupObjects;
    public float lengthAbovePlatform = 1;
    public float chanceToSpawnPowerup = 0.1f;

    IEnumerator Start()
    {
        cam = Camera.main;


        yield return null;


        float[] originalPlatformDimensions = FindObjectDimensions(platformObject);

        originalPlatformWidth = originalPlatformDimensions[0];
        originalPlatformHeight = originalPlatformDimensions[1];

        isReady = true;


        lastSpawnY = -distanceBetween;
    }

    void Update()
    {
        if (!isReady) return;


        if (cam.transform.position.y - lastSpawnY >= distanceBetween)
        {
            lastSpawnY = cam.transform.position.y;
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
                        //float[] platformDimensions = FindObjectDimensions(platform);
                        if (Mathf.Abs(newPlatformPositionX - platform.transform.position.x) < platformMaxScale * 2 + safetyDistance)
                        {
                            overlapping = true;
                            nbrTries++;
                            break;
                        }
                    }


                    if (nbrTries >= triesSpawningPlatform && overlapping)
                    {
                        skipPlatform = true;
                        break;
                    }
                } while (overlapping);

                if (!skipPlatform)
                {
                    GameObject newPlatform = Instantiate(platformObject, new Vector2(newPlatformPositionX, CameraScript.screenHeight + originalPlatformHeight + cam.transform.position.y), Quaternion.identity);
                    spawnedPlatforms.Add(newPlatform);

                    int maxRange = Mathf.RoundToInt(1 / chanceToSpawnPowerup);
                    if (Random.Range(0, maxRange) == 0)
                        SpawnPowerup(newPlatform);

                    platformsColliders.Add(newPlatform.GetComponent<Collider2D>());
                }
            }
        }

    }

    public float[] FindObjectDimensions(GameObject gameObject)
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

