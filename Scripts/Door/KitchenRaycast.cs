using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenRaycast : MonoBehaviour
{
    [SerializeField] private int rayDistance = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Schimbat de la Mouse0 la E
    [SerializeField] private Image crosshair = null;
    
    private ChitckenOpen raycastedObj;
    private bool isCrosshairActive;
    private bool doOnce;
    private const string interactableTag = "InteractiveObject";

    private void Update()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
        
        if(Physics.Raycast(transform.position, forward, out hit, rayDistance, mask))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                if (!doOnce)
                {
                    raycastedObj = hit.collider.gameObject.GetComponent<ChitckenOpen>();
                    CrosshairChange(true);
                }
                isCrosshairActive = true;
                doOnce = true;
                
                if (Input.GetKeyDown(interactKey))
                {
                    raycastedObj.PlayAnimation(); // Folosește variabila locală
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
    
    void CrosshairChange(bool on)
    {
        if (on && !doOnce)
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
