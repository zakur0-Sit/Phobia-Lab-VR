using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioSource footsteepsSound;
    public float normalPitch = 1.0f;
    public float sprintPitch = 1.4f;

    void Update()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                footsteepsSound.pitch = sprintPitch;
            }
            else
            {
                footsteepsSound.pitch = normalPitch;
            }

            if (!footsteepsSound.isPlaying)
                footsteepsSound.Play();
        }
        else
        {
            footsteepsSound.Stop();
        }
    }
}