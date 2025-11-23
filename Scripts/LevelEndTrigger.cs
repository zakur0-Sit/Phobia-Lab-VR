using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    public string nextSceneName;
    public FadeController fadeController; // Referință la FadeController
    public float delayAfterFade = 2f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TransitionToNextScene());
        }
    }

    private IEnumerator TransitionToNextScene()
    {
        // Pornim fade-out
        yield return fadeController.StartCoroutine(fadeController.FadeOut());
        yield return new WaitForSeconds(delayAfterFade);
        
        // Încărcăm scena nouă
        SceneManager.LoadScene(nextSceneName);

        // Fade-in-ul se va executa automat în noua scenă (din Start() din FadeController)
    }
}