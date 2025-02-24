using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    IPlayerState currentState;

    public Animator animator { get; set; }
    public Rigidbody rigid { get; set; }
    public float facingRight { get; set; }
    public bool isFloor { get; set; }

    public float turnSpeed = 80.0f;
    public float moveSpeed = 10.0f;
    public float jumpHeight = 2.0f;

    [SerializeField] GameObject playerWeapon;

    Rigidbody weaponRigid;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        weaponRigid = playerWeapon.GetComponent<Rigidbody>();
    }

    void Start()
    {
        rigid.freezeRotation = true;
        isFloor = true;
        weaponRigid.isKinematic = true;
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
        bool isKnifeReturn = true;

        return isKnifeReturn;
    }

    public void ThrowKnifeAttack()
    {
        StartCoroutine(ThrowKnife());
        //weaponRigid.isKinematic = false;
        //weaponRigid.gameObject.transform.parent = null;
        //weaponRigid.AddForce(transform.forward * 10, ForceMode.Impulse);
    }

    IEnumerator ThrowKnife()
    {
        yield return new WaitForSeconds(0.35f);
        weaponRigid.isKinematic = false;
        weaponRigid.gameObject.transform.parent = null;
        weaponRigid.AddForce(transform.forward * 10, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isFloor = true;
        }
    }
}
