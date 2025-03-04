using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public bool isTaskComplete { get; set; }
    public PickUpObjectType objectType { get; private set; }
    public GameObject pickUpObject { get; private set; }
    public GameObject pickUp2DObject { get; private set; }

    PlayerMovement playerMove;
    ObjectController pickUpObj;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerMove = other.GetComponent<PlayerMovement>();
        }

        if(other.CompareTag("Movable"))
        {
            if(other.gameObject.GetComponent<ObjectController>() != null)
            {
                pickUpObject = other.gameObject;
                pickUpObj = pickUpObject.GetComponent<ObjectController>();
                objectType = pickUpObj.objectType.type;
                pickUp2DObject = pickUpObj.objectType.pick2DObject;
            }
            isTaskComplete = true;
        }
    }
}
