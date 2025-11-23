using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeySystem
{
    public class KeyDoorController : MonoBehaviour
    {
        private Animator doorAnimator;
        private bool isOpen = false;
        private bool isLocked = false; 
        
        [Header("Animation Names")]
        [SerializeField] private string openAnimationName = "DoorOpen";
        [SerializeField] private string closeAnimationName = "DoorClose";
        [SerializeField] private int timeToShowUI = 1;
        [SerializeField] private GameObject showDoorLockedUI = null;
        [SerializeField] private KeyInventory keyInventory = null;
        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;
        
        [SerializeField] private AudioSource doorOpen; 
        [SerializeField] private float openDelay = 0;
        [SerializeField] private AudioSource doorClose;
        [SerializeField] private float closeDelay = 0;
        
        private void Awake()
        {
            doorAnimator = gameObject.GetComponent<Animator>();
        }
        
        private IEnumerator PauseDoorInteraction()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;
        }
        
        public void PlayAnimation()
        {
            
            if (isLocked) return; // Nu face nimic dacă e blocată
            
            if(keyInventory.hasKey)
            {
                OpenDoor();
            }
            else
            {
                StartCoroutine(ShowDoorLocked());
            }
        }

        void OpenDoor()
        {
            if (!isOpen && !pauseInteraction)
            {
                doorAnimator.Play(openAnimationName, 0, 0.0f);
                isOpen = true;
                StartCoroutine(PauseDoorInteraction());
                doorOpen.PlayDelayed(openDelay);
            }
            else if(isOpen && !pauseInteraction)
            {
                doorAnimator.Play(closeAnimationName, 0, 0.0f);
                isOpen = false;
                StartCoroutine(PauseDoorInteraction());
                doorClose.PlayDelayed(closeDelay);
                
            }
        }
        
        IEnumerator ShowDoorLocked()
        {
            showDoorLockedUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowUI);
            showDoorLockedUI.SetActive(false);
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
}