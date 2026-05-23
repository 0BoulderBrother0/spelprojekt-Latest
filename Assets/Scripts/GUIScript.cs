using System.Collections;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour
{
    public static TextMeshProUGUI score;
    public static TextMeshProUGUI lose;
    public static GUIScript instance;

    public static Image jumpPowerup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("JumpPowerupUI"))
                jumpPowerup = child.GetComponent<Image>();

            if (child.CompareTag("ScoreUI"))
                score = child.GetComponent<TextMeshProUGUI>();

            if (child.CompareTag("LoseUI"))
                lose = child.GetComponent<TextMeshProUGUI>();
        }
    }


    public static IEnumerator FadeIcon(Image img, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Color c = img.color;
            c.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            img.color = c;
            yield return null;
        }


        Color final = img.color;
        final.a = 0f;
        img.color = final;
    }

    public static IEnumerator ShowText(TextMeshProUGUI tmp, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Color c = tmp.color;
            c.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            tmp.color = c;
            yield return null;
        }


        Color final = tmp.color;
        final.a = 1f;
        tmp.color = final;
    }
}
