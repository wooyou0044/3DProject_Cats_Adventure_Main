using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject door;
    PlayerMovement player;
    Collider doorCol;
    Collider doorCol3D;
    Rigidbody doorRigid;
    Rigidbody myRigid;
    Animator doorAni;

    private void Awake()
    {
        doorAni = GetComponent<Animator>();
        doorCol = GetComponent<Collider>();
        doorRigid = door.GetComponent<Rigidbody>();
        myRigid = GetComponent<Rigidbody>();
        doorCol3D = door.GetComponent<Collider>();
    }

    void Start()
    {
        doorAni.enabled = false;
        doorRigid.isKinematic = true;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(player == null)
            {
                player = collision.gameObject.GetComponent<PlayerMovement>();
            }

            if (player.ReturnIsHaveKey() == true)
            {
                doorAni.enabled = true;
                doorCol.isTrigger = true;
                player.TurnOffPortal();
                StartCoroutine(DoorOpen());
            }
        }
    }

    IEnumerator DoorOpen()
    {
        yield return new WaitForSeconds(0.2f);
        doorCol.isTrigger = true;
        doorRigid.isKinematic = false;
        doorCol3D.isTrigger = true;
        myRigid.constraints = RigidbodyConstraints.None;
        myRigid.isKinematic = true;
        doorRigid.AddForce(Vector3.forward, ForceMode.Impulse);
        player.SetIsHaveKey(false);
        StartCoroutine(DoorOpening());
    }

    IEnumerator DoorOpening()
    {
        player.SetKeyActive(false);
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        doorCol3D.isTrigger = false;
    }
}
