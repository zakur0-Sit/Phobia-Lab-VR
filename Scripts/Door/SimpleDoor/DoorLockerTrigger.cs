using System.Collections;
using System.Collections.Generic;
using KeySystem;
using UnityEngine;

using UnityEngine;

public class DoorLockerTrigger : MonoBehaviour
{
    [SerializeField] private DoorController targetDoor;
    [SerializeField] private KeyDoorController targetKeyDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetDoor != null)
        {
            targetDoor.LockDoor();
            Destroy(gameObject); 
        }
        if (other.CompareTag("Player") && targetKeyDoor != null)
        {
            targetKeyDoor.LockDoor();
            Destroy(gameObject); 
        }
    }
}
