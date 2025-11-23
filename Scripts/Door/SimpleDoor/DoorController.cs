using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator;
    private bool isOpen = false;
    private bool isLocked = false; 
    [SerializeField] private AudioSource doorOpen; 
    [SerializeField] private float openDelay = 0;
    [SerializeField] private AudioSource doorClose;
    [SerializeField] private float closeDelay = 0;
    

    private void Awake()
    {
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (isLocked) return; 

        if (!isOpen)
        {
            doorAnimator.Play("DoorOpen", 0, 0.0f);
            isOpen = true;
            doorOpen.PlayDelayed(openDelay);
        }
        else
        {
            doorAnimator.Play("DoorClose", 0, 0.0f);
            isOpen = false;
            doorClose.PlayDelayed(closeDelay);
        }
    }

    public void LockDoor()
    {
        if (isOpen)
        {
            doorAnimator.Play("DoorClose", 0, 0.0f);
            isOpen = false;
            doorClose.PlayDelayed(closeDelay);
        }
        isLocked = true;
    }
}
