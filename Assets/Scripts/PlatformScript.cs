using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    SpriteRenderer[] srArray;

    public float platformWidth;
    public float platformHeight;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        transform.localScale = new Vector3(Random.Range(1f, PlatformManagerScript.platformMaxScale), 1, 1);


        srArray = GetComponentsInChildren<SpriteRenderer>(); 

        Bounds totalBounds = srArray[0].bounds;
        foreach (SpriteRenderer sr in srArray)
        {
            totalBounds.Encapsulate(sr.bounds);
        }

        platformWidth = totalBounds.extents.x;
        platformHeight = totalBounds.extents.y;
    }
}
