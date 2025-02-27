using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    public enum ActionState
    {
        None,
        NPC,
        Movable,
        PlaceObject,
        Portal,
        Wire
    }
    
    IPlayerState currentState;

    public Animator animator { get; set; }
    public Rigidbody rigid { get; set; }
    public bool isFloor { get; set; }
    public bool isThrow { get; set; }
    public bool isColliderActive { get; set; }
    public bool isCanCooperate { get; set; }
    public bool isConverseEnd { get; private set; }

    public bool isPressEnter { get; set; }
    public bool isPickUpAlready { get; set; }
    public bool isPutDownAlready { get; set; }


    public float turnSpeed = 80.0f;
    public float moveSpeed = 10.0f;
    public float jumpHeight = 2.0f;
    public int poolSize = 10;

    [SerializeField] GameObject gameManagerObject;
    [SerializeField] GameObject playerWeapon;
    [SerializeField] Transform weaponPos;
    [SerializeField] Transform eyeTrans;
    [SerializeField, Range(0.5f, 3f)] float weaponThrowPower;

    GameManager gameManager;

    GameObject throwWeapon;
    GameObject hitColliderObject;

    Rigidbody throwWeaponRigid;
    Collider collider;
    Collider objectCol;

    public ActionState currentAction { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    void Start()
    {
        rigid.freezeRotation = true;
        isFloor = true;
        isThrow = false;
        // 던질 무기
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
        int layerMask = LayerMask.GetMask("NPC");

        if(Physics.Raycast(ray, out hit, 2f, layerMask))
        {
            isCanCooperate = true;
            currentAction = ActionState.NPC;
            objectCol = hit.collider;
            hitColliderObject = hit.collider.gameObject;
            gameManager.SetPopUpUIType(layerMask);
            gameManager.SetPopUpActive(true, hit.collider.transform);
        }
        else
        {
            if(isCanCooperate == true && currentAction != ActionState.PlaceObject)
            {
                Debug.Log("Ray : " + isCanCooperate);
                currentAction = ActionState.None;
                isCanCooperate = false;
            }

            if (gameManager.isCanvasOpen() == true)
            {
                gameManager.SetPopUpActive(false);
            }
        }
    }

    public GameObject SetHitObject()
    {
        return hitColliderObject == null ? null : hitColliderObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 start = eyeTrans.position;
        Vector3 direction = eyeTrans.forward * 3f;
        Vector3 end = start + direction;

        Gizmos.DrawLine(start, end);
    }


    public void ConversationWithNPC(int num)
    {
        isConverseEnd = gameManager.SendConversationEnd();
        isPressEnter = true;
        gameManager.SetNPCTalkCanvasActive(true);
        gameManager.SetConversationInUI(num);

    }

    public bool GetIsConverseAlready()
    {
        return gameManager.SendMessageAllRepresent();
    }

   public void ChangeActionMode(ActionState state)
    {
        currentAction = state;
    }

    public bool CompareSizeBigger()
    {
        if(objectCol.bounds.size.y >= collider.bounds.size.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PickUp()
    {
        StartCoroutine(MakePickUpObject());
    }

    IEnumerator MakePickUpObject()
    {
        yield return new WaitForSeconds(0.85f);
        playerWeapon.SetActive(false);
        hitColliderObject.GetComponent<Rigidbody>().isKinematic = true;
        hitColliderObject.transform.SetParent(weaponPos);
        hitColliderObject.transform.localPosition = new Vector3(0.4f, 0, 0.25f);
        hitColliderObject.transform.localRotation = Quaternion.Euler(0,0,180);
        hitColliderObject.GetComponent<Collider>().isTrigger = true;
        isPickUpAlready = true;
    }

    public void PutDown()
    {
        StartCoroutine(PutDownObject());
    }

    IEnumerator PutDownObject()
    {
        yield return new WaitForSeconds(0.25f);
        hitColliderObject.transform.parent = null;
        hitColliderObject.GetComponent<Collider>().isTrigger = false;
        //hitColliderObject.transform.position = playerWeapon.transform.position + new Vector3(0.4f, 0, 0.2f);
        hitColliderObject.GetComponent<Rigidbody>().isKinematic = false;
        playerWeapon.SetActive(true);
        isPutDownAlready = true;
    }

    public void MoveObject(Vector3 dir, float speed)
    {
        hitColliderObject.transform.Translate(dir * speed * Time.deltaTime * 0.5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isFloor = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Movable"))
        {
            isCanCooperate = true;
            isColliderActive = true;
            if(currentAction != ActionState.PlaceObject)
            {
                currentAction = ActionState.Movable;
                objectCol = collision.collider;
                hitColliderObject = collision.collider.gameObject;
                gameManager.SetPopUpUIType(LayerMask.GetMask("Movable"));
            }
            else
            {
                gameManager.SetUIPutObject();
            }
            gameManager.SetPopUpActive(true, collision.collider.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(isCanCooperate == true && currentAction == ActionState.Movable)
        {
            currentAction = ActionState.None;
            isCanCooperate = false;
        }

        if(isColliderActive == true)
        {
            isColliderActive = false;
        }
    }
}
