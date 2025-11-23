using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeySystem
{
    public class KeyItemController : MonoBehaviour
    {
        [SerializeField] private bool isdoor = false;
        [SerializeField] private bool iskey = false;
        [SerializeField] private KeyInventory keyInventory = null;
        [SerializeField] private AudioSource keyPickupSound; 
        
        private KeyDoorController doorObject;
        
        private void Start()
        {
            if (isdoor)
            {
                doorObject = GetComponent<KeyDoorController>();       
            }
        }

        public void ObjectInteraction()
        {
            if (isdoor)
            {
                doorObject.PlayAnimation();
            }
            else if(iskey)
            {
                keyInventory.hasKey = true;
                gameObject.SetActive(false);
                keyPickupSound.Play();
            }
        }
    }
}