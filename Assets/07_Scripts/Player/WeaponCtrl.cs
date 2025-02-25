using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCtrl : MonoBehaviour
{
    public bool isBumped { get; set; }

    [SerializeField]PlayerMovement playerMove;
    Rigidbody rigid;
    Collider collider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        TurnOffCollider(true);
    }
    void Start()
    {
        
    }

    public void TurnOffCollider(bool isOn)
    {
        collider.isTrigger = isOn;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "player")
        {
            isBumped = true;
            playerMove.isThrow = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(playerMove == null)
            {
                playerMove = other.gameObject.transform.root.GetComponent<PlayerMovement>();
            }
        }

        if (other.gameObject.tag == "Player" && playerMove.isThrow == true)
        {
            isBumped = false;
            playerMove.SetWeaponInHand();
        }
    }
}
