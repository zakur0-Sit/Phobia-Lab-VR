using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Referință către Image-ul de fade
    public float fadeSpeed = 1f; // Viteza de fade

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float alpha = 0f;
        fadeImage.color = new Color(0, 0, 0, alpha);

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
    
    public IEnumerator FadeIn()
    {
        float alpha = 1f;
        fadeImage.color = new Color(0, 0, 0, alpha);

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}