using UnityEngine;
using UnityEngine.UI;

public class DoorRaycast : MonoBehaviour
{
    [SerializeField] private int rayDistance = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Schimbat de la Mouse0 la E
    [SerializeField] private Image crosshair = null;
    
    private DoorController raycastedObj;
    private bool isCrosshairActive;
    private bool doOnce;
    private const string interactableTag = "InteractiveObject";

    private void Update()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        
        int mask = excludeLayerName != null 
            ? (1 << LayerMask.NameToLayer(excludeLayerName)) | layerMaskInteract.value
            : layerMaskInteract.value;
        
        if(Physics.Raycast(transform.position, forward, out hit, rayDistance, mask))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                DoorController doorController = hit.collider.GetComponent<DoorController>();
                if (doorController != null)
                {
                    if (!doOnce)
                    {
                        raycastedObj = doorController;
                        CrosshairChange(true);
                    }
                    isCrosshairActive = true;
                    doOnce = true;
                    
                    if (Input.GetKeyDown(interactKey))
                    {
                        doorController.PlayAnimation(); // Folosește variabila locală
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
        if (crosshair != null) 
        {
            crosshair.color = isActive ? Color.red : Color.white;
            isCrosshairActive = isActive;
        }
    }
}