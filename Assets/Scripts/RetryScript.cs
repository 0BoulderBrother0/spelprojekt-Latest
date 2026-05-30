using UnityEngine;
using UnityEngine.SceneManagement;


public class RetryScript : MonoBehaviour
{
    public void StartGame()
    {
        PlayerScript.endGame = false;
        PlayerScript.score = 0;
        PlayerScript.retries++;
        GUIScript.ranGameOver = false;
        GUIScript.ranButton = false;
        SceneManager.LoadScene("GameScene");
    }
}
