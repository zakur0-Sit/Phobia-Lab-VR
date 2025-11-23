using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitckenOpen : MonoBehaviour
{
    private Animator doorAnimator;
    private bool isOpen = false;
    private bool isLocked = false;


    private void Awake()
    {
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (!isOpen)
        {
            doorAnimator.Play("Opening", 0, 0.0f);
            isOpen = true;
        }
        else
        {
            doorAnimator.Play("Closing", 0, 0.0f);
            isOpen = false;
        }
    }
}