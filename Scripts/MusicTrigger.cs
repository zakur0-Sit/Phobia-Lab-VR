using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioSource currentMusicSource;
    public AudioSource newMusicSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentMusicSource != null && currentMusicSource.isPlaying)
                currentMusicSource.Stop();

            if (newMusicSource != null && !newMusicSource.isPlaying)
                newMusicSource.Play();
        }
    }
}