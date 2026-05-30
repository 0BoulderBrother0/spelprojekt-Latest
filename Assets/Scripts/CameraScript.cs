using UnityEditor.Rendering;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Speed Settings")]
    public float baseCameraSpeed;
    public float jumpPowerupSpeed;
    public float playerMoveFactor;
    public float thresholdToMove = 7;
    public float speedScoreKoefficient = 100;
    public static float progressionKoefficient;

    float currentCameraSpeed;
    public float gameOverSlowdown = 0.1f;
    float gameOverSpeed;
    bool assignedGameOverSpeed;
    public float cameraStandStillThreshold;
    public float resumeGameSpeedup = 1.3f;
    public float resumeGameBaseSpeedThreshold = 0.01f;
    public float resumeGameToPlayerPositionRegulationDistance = 0.5f;

    public static float screenWidth;
    public static float screenHeight;

    Camera cam;
    PlayerScript ps;

    Vector2 playerCameraVector;
    public static float overstepDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        screenHeight = cam.orthographicSize;
        screenWidth = screenHeight * cam.aspect;

        currentCameraSpeed = baseCameraSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerCameraVector = ps.playerPos - new Vector2(transform.position.x, transform.position.y);

        float threshold = screenHeight - (ps.playerHeight * thresholdToMove);

        progressionKoefficient = Mathf.Max(1, PlayerScript.score / speedScoreKoefficient);


        if (PlayerScript.endGame)
        {
            if (!assignedGameOverSpeed)
            {
                assignedGameOverSpeed = true;
                gameOverSpeed = currentCameraSpeed + progressionKoefficient;
            }


            gameOverSpeed = Mathf.Lerp(gameOverSpeed, 0, gameOverSlowdown / progressionKoefficient * Time.deltaTime);
            if (gameOverSpeed <= cameraStandStillThreshold)
            {
                gameOverSpeed = 0;
            }

            transform.position += new Vector3(0, gameOverSpeed, 0) * Time.deltaTime;
        }
        else
        {
            if (PlayerScript.resumeGame)
            {
                if (playerCameraVector.y >= threshold)
                {
                    overstepDistance = playerCameraVector.y - threshold;
                    currentCameraSpeed = Mathf.Lerp(currentCameraSpeed, baseCameraSpeed + (overstepDistance * playerMoveFactor), resumeGameSpeedup * Time.deltaTime);
                }
                else
                {
                    currentCameraSpeed = Mathf.Lerp(currentCameraSpeed, baseCameraSpeed, resumeGameSpeedup * Time.deltaTime);
                }

                if (currentCameraSpeed >= baseCameraSpeed)
                {
                    PlayerScript.resumeGame = false;
                    Debug.Log($"Resume game: {PlayerScript.resumeGame}");
                }
            }
            else
            {
                if (ps.jumpPowerupActive != null && playerCameraVector.y >= threshold)
                {
                    overstepDistance = playerCameraVector.y - threshold;
                    currentCameraSpeed = baseCameraSpeed + (overstepDistance * playerMoveFactor * jumpPowerupSpeed);
                }
                else if (ps.temporarilyIgnoreGround == null && playerCameraVector.y >= threshold)
                {
                    overstepDistance = playerCameraVector.y - threshold;
                    currentCameraSpeed = baseCameraSpeed + (overstepDistance * playerMoveFactor);
                }
                else
                {
                    currentCameraSpeed = baseCameraSpeed;
                }
            }

            transform.position += new Vector3(0, currentCameraSpeed + progressionKoefficient, 0) * Time.deltaTime;
        }

    }
}