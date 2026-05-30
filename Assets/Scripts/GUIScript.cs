using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour
{
    public static TextMeshProUGUI score;
    public static TextMeshProUGUI lose;
    public static TextMeshProUGUI highscore;
    public static Image retry;
    public static GUIScript instance;

    public static Image jumpPowerup;
    public static Image invincibilityPowerup;

    public static bool ranButton;
    public static TextMeshProUGUI imgText;
    public static bool ranGameOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("JumpPowerupUI"))
                jumpPowerup = child.GetComponent<Image>();

            if (child.CompareTag("InvincibilityPowerupUI"))
                invincibilityPowerup = child.GetComponent<Image>();

            if (child.CompareTag("ScoreUI"))
                score = child.GetComponent<TextMeshProUGUI>();

            if (child.CompareTag("LoseUI"))
                lose = child.GetComponent<TextMeshProUGUI>();

            if (child.CompareTag("HighscoreUI"))
                highscore = child.GetComponent<TextMeshProUGUI>();

            if (child.CompareTag("RetryUI"))
                retry = child.GetComponent<Image>();
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
        if (ranGameOver)
            yield return new WaitForSeconds(duration);

        float elapsed = 0f;
        if (tmp.CompareTag("LoseUI"))
            ranGameOver = true;


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

    public static IEnumerator ShowImage(Image img, float duration)
    {
        if (ranGameOver)
            yield return new WaitForSeconds(duration);

        float elapsed = 0f;
        if (img.CompareTag("RetryUI"))
        {
            imgText = img.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            ranButton = true;
        }


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Color c = img.color;
            c.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            img.color = c;

            if (ranButton)
            {
                Color cText = imgText.color;
                cText.a = Mathf.Lerp(0f, 1f, elapsed / duration);
                imgText.color = cText;
            }

            yield return null;
        }


        Color final = img.color;
        final.a = 1f;
        img.color = final;

        if (ranButton)
            img.gameObject.GetComponent<Button>().interactable = true;
    }
}
