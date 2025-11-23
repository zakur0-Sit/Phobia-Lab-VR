using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeySystem
{
    public class KeyRayCast : MonoBehaviour
    {
        [SerializeField] private int rayDistance = 5;
        [SerializeField] private LayerMask layerMaskInteract;
        [SerializeField] private string excludeLayerName = null;
        [SerializeField] private KeyCode openDoorKey = KeyCode.Mouse0;
        [SerializeField] private Image crosshair = null;
        
        private KeyItemController raycastedObject;
        private bool isCrosshairActive;
        private bool doOnce;
        private string interactableTag = "InteractiveObject";

        private void Update()
        {
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            int mask = layerMaskInteract.value;

            if (Physics.Raycast(transform.position, forward, out hit, rayDistance, mask))
            {
                if (hit.collider != null && hit.collider.CompareTag(interactableTag))
                {
                    KeyItemController controller = hit.collider.GetComponent<KeyItemController>();
                    if (controller != null) // Verifică existența componentei
                    {
                        if (!doOnce)
                        {
                            raycastedObject = controller;
                            CrosshairChange(true);
                        }
                        isCrosshairActive = true;
                        doOnce = true;

                        if (Input.GetKeyDown(openDoorKey))
                        {
                            controller.ObjectInteraction(); // Folosește controllerul local
                        }
                    }
                }
            }
            else
            {
                if (isCrosshairActive)
                {
                    CrosshairChange(false);
                    doOnce = false;
                }
            }
        }
        void CrosshairChange(bool isActive)
        {
            if (isActive && !doOnce)
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
                isCrosshairActive = false;
            }
        }
    }
}