using System.Collections;
using UnityEngine;

public class TriggerRandomSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public float minInterval = 5f;
    public float maxInterval = 10f;
    public string playerTag = "Player";

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag(playerTag))
        {
            hasTriggered = true;
            StartCoroutine(PlayRandomSoundLoop());
        }
    }

    private IEnumerator PlayRandomSoundLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (clips.Length > 0)
            {
                int index = Random.Range(0, clips.Length);
                audioSource.PlayOneShot(clips[index]);
            }
        }
    }
}