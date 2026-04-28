using System.Collections;
using UnityEngine;

public class PlatformManagerScript : MonoBehaviour
{
    public GameObject platformObject;
    public float interval;

    
    SpriteRenderer sr;
    Coroutine spawnPlatforms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        sr = platformObject.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnPlatforms()
    {
        while (true)
        {
            float platformPositionX = Random.Range(-CameraScript.screenWidth + sr.bounds.extents.x, CameraScript.screenWidth - sr.bounds.extents.x);
            Debug.Log(platformPositionX);

            Instantiate(platformObject, new Vector2(platformPositionX, CameraScript.screenHeight - sr.bounds.extents.y), Quaternion.identity);

            yield return new WaitForSeconds(interval);
        }
    }
}
