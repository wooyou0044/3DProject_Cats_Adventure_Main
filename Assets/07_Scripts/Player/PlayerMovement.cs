using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    IPlayerState currentState;

    public Animator animator { get; set; }
    public Rigidbody rigid { get; set; }
    public bool isFloor { get; set; }
    public bool isThrow { get; set; }

    public float turnSpeed = 80.0f;
    public float moveSpeed = 10.0f;
    public float jumpHeight = 2.0f;
    public int poolSize = 10;

    [SerializeField] GameObject playerWeapon;
    [SerializeField] Transform weaponPos;
    [SerializeField] Transform eyeTrans;
    [SerializeField, Range(0.5f, 3f)] float weaponThrowPower;

    GameObject throwWeapon;

    Rigidbody throwWeaponRigid;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rigid.freezeRotation = true;
        isFloor = true;
        isThrow = false;
        // ´øÁú ¹«±â
        throwWeapon = Instantiate(playerWeapon, weaponPos.position, weaponPos.rotation);
        throwWeaponRigid = throwWeapon.GetComponent<Rigidbody>();
        throwWeapon.SetActive(false);

        ChangeState(new IdleState());
    }

    void Update()
    {
        currentState.UpdateState(this);

    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void ChangeState(IPlayerState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public bool CanJump()
    {
        return isFloor;
    }

    public bool CanThrow()
    {
        return (isThrow == false)? true : false;
    }

    public void ThrowWeaponAttack()
    {
        StartCoroutine(ThrowWeapon());
    }

    IEnumerator ThrowWeapon()
    {
        yield return new WaitForSeconds(0.35f);
        playerWeapon.SetActive(false);
        if (throwWeapon != null)
        {
            throwWeapon.transform.position = weaponPos.position;
            throwWeapon.transform.rotation = weaponPos.rotation;
            throwWeapon.SetActive(true);

            throwWeapon.GetComponent<WeaponCtrl>().TurnOffCollider(false);
            throwWeaponRigid.isKinematic = false;
            throwWeaponRigid.constraints = RigidbodyConstraints.FreezePositionY;
            throwWeaponRigid.AddForce(transform.forward * 10, ForceMode.Impulse);
            throwWeaponRigid.AddTorque(new Vector3(1, 1, 1) * 1000, ForceMode.Impulse);
        }

        StartCoroutine(ReturnWeapon());
    }

    public void ReturnWeaponInHand()
    {
        throwWeapon.GetComponent<WeaponCtrl>().TurnOffCollider(true);
        throwWeaponRigid.constraints = RigidbodyConstraints.None;
        Vector3 destination = (weaponPos.position - throwWeapon.transform.position).normalized;
        throwWeaponRigid.velocity = destination * 20f;
    }

    public void SetWeaponInHand()
    {
        throwWeapon.SetActive(false);
        throwWeaponRigid.isKinematic = true;
        throwWeapon.transform.position = weaponPos.position;
        throwWeapon.transform.rotation = weaponPos.rotation;
        throwWeapon.GetComponent<WeaponCtrl>().isBumped = false;
        StopAllCoroutines();
        isThrow = false;
        playerWeapon.SetActive(true);
    }

    IEnumerator ReturnWeapon()
    {
        if (throwWeapon.GetComponent<WeaponCtrl>().isBumped == true)
        {
            yield break;
        }
        yield return new WaitForSeconds(weaponThrowPower);
        if (throwWeapon.GetComponent<WeaponCtrl>().isBumped == false)
        {
            isThrow = true;
        }
    }

    public void DoEyeSight()
    {
        Ray ray = new Ray(eyeTrans.position, transform.forward);
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("NPC", "Movable");

        if(Physics.Raycast(ray, out hit, 5f, layerMask))
        {
            Debug.Log("ºÎµúÇûÀ½");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isFloor = true;
        }
    }
}
