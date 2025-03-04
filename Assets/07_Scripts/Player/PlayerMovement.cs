using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

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

    public enum PlayerMode
    {
        Mode_2D,
        Mode_3D
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
    public bool isInPortal { get; set; }
    public bool isComplete { get; set; }


    public float turnSpeed = 80.0f;
    public float moveSpeed = 10.0f;
    public float jumpHeight = 2.0f;
    public int poolSize = 10;

    [SerializeField] PlayerMode currentMode;
    [SerializeField] int normalAttackNum = 5;
    [SerializeField] int throwAttackNum = 7;

    [Header("3D")]
    [SerializeField] GameObject gameManagerObject;
    [SerializeField] GameObject playerWeapon;
    [SerializeField] Transform weaponPos;
    [SerializeField] Transform eyeTrans;
    [SerializeField, Range(0.5f, 3f)] float weaponThrowPower;

    [Header("2D")]
    SkeletonMecanim skeletonMecanim;
    [SpineSlot] public string slotNameToHide;
    [SpineSlot] public string[] weaponName;
    [SpineSlot] public string armName;

    GameManager gameManager;
    public PortalController portalCtrl { get; private set; }

    GameObject throwWeapon;
    GameObject hitColliderObject;

    Rigidbody throwWeaponRigid;
    Collider collider;
    Collider objectCol;

    Bone bone;

    public Transform attachObjectPos { get; set; }

    public ActionState currentAction { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        if(currentMode == PlayerMode.Mode_2D)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            skeletonMecanim = animator.GetComponent<SkeletonMecanim>();
        }
        else
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
    }

    void Start()
    {
        rigid.freezeRotation = true;
        isFloor = true;
        isThrow = false;

        if(currentMode == PlayerMode.Mode_3D)
        {
            // 던질 무기
            throwWeapon = Instantiate(playerWeapon, weaponPos.position, weaponPos.rotation);
            throwWeaponRigid = throwWeapon.GetComponent<Rigidbody>();
            throwWeapon.SetActive(false);
            ChangeState(new IdleState());
        }

        else
        {
            MakeSlotHide(slotNameToHide);
            ChangeState(new IdleState2D());
        }

        //ChangeState(new IdleState());
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
                currentAction = ActionState.None;
                isCanCooperate = false;
            }

            if (gameManager.isCanvasOpen() == true && currentAction == ActionState.None)
            {
                gameManager.SetPopUpActive(false);
            }
        }
    }

    public GameObject SetHitObject()
    {
        return hitColliderObject == null ? null : hitColliderObject;
    }

    public float GetHitObjectSizeY()
    {
        if(objectCol == null)
        {
            return 0;
        }
        return objectCol.bounds.size.y;
    }

    public void TurnOnNPCAnimator(bool isActive)
    {
        hitColliderObject.transform.LookAt(gameObject.transform.position);
        hitColliderObject.transform.GetComponentInChildren<Animator>().enabled = isActive;

        if (isActive == false)
        {
            isConverseEnd = false;
            currentAction = ActionState.None;
        }
    }

    private void OnDrawGizmos()
    {
        if(currentMode == PlayerMode.Mode_3D)
        {
            Gizmos.color = Color.red;

            Vector3 start = eyeTrans.position;
            Vector3 direction = eyeTrans.forward * 3f;
            Vector3 end = start + direction;

            Gizmos.DrawLine(start, end);
        }
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
        if (objectCol.bounds.size.y >= collider.bounds.size.y)
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
        //playerWeapon.SetActive(false);
        //hitColliderObject.GetComponent<Rigidbody>().isKinematic = true;
        //hitColliderObject.transform.SetParent(weaponPos);
        //hitColliderObject.transform.localPosition = new Vector3(0.4f, 0, 0.25f);
        //hitColliderObject.transform.localRotation = Quaternion.Euler(0,0,180);
        //hitColliderObject.GetComponent<Collider>().isTrigger = true;
        MakePickUpObject(hitColliderObject);
        isPickUpAlready = true;
    }

    public void MakePickUpObject(GameObject gameObject, bool isWorldChange = false)
    {
        if(isWorldChange == true)
        {
            objectCol = gameObject.GetComponent<Collider>();
        }
        playerWeapon.SetActive(false);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.SetParent(weaponPos);
        if(isWorldChange == false)
        {
            gameObject.transform.localPosition = new Vector3(0.4f, 0, 0.25f);
        }
        else
        {
            if(gameManager.pickUpType == PickUpObjectType.Trampoline)
            {
                gameObject.transform.localPosition = new Vector3(0.4f, 0, 0.25f);
            }
            else
            {
                gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
        gameObject.GetComponent<Collider>().isTrigger = true;
    }

    public void PutDown(bool isComplete = false)
    {
        if(isComplete == true)
        {
            hitColliderObject = objectCol.gameObject;
        }
        StartCoroutine(PutDownObject());
    }

    IEnumerator PutDownObject()
    {
        yield return new WaitForSeconds(0.2f);
        hitColliderObject.transform.parent = null;
        hitColliderObject.GetComponent<Collider>().isTrigger = false;

        StartCoroutine(TurnOffKinematic());
        //hitColliderObject.GetComponent<Rigidbody>().isKinematic = false;
        //playerWeapon.SetActive(true);
        isPutDownAlready = true;
    }

    IEnumerator TurnOffKinematic()
    {
        yield return new WaitForSeconds(0.2f);
        hitColliderObject.GetComponent<Rigidbody>().isKinematic = false;
        playerWeapon.SetActive(true);
    }

    public void MoveObject(Vector3 dir, float speed)
    {
        hitColliderObject.transform.Translate(dir * speed * Time.deltaTime * 0.5f);
    }

    public void MoveIntoOtherWorld(bool is3D = true)
    {
        StartCoroutine(MoveToPortal(is3D));
    }

    IEnumerator MoveToPortal(bool is3D = true)
    {
        gameManager.SetPopUpUIByType(currentAction, false);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        if(is3D == true)
        {
            // 임시 2D Player 꺼내기 => mapNum 만들어야 함 (json이나 데이터 저장하는걸로 확인)
            gameManager.Make2DPlayer();
            // 키 들고 들어가면 손에 생성한채로 들어감
        }
        else
        {
            portalCtrl.gameObject.SetActive(false);
            portalCtrl.pickUpObject.SetActive(false);
            gameManager.pickUpType = portalCtrl.objectType;
            gameManager.Set3DObjectActive(true);
            portalCtrl.isTaskComplete = false;
            gameManager.currentMapNum++;
        }
        currentAction = ActionState.None;
        gameManager.SetPopUpActive(false);
    }

    public bool ReturnIsHaveKey()
    {
        return gameManager.isHaveKey;
    }

    public void SetIsHaveKey(bool isActive)
    {
        gameManager.isHaveKey = isActive;
    }

    #region 2D
    public void StopJumping()
    {
        StartCoroutine(Jumping());
    }

    IEnumerator Jumping()
    {
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = new Vector2(0, 0);
    }

    public void MakeSlotHide(string slotName)
    {
        if (skeletonMecanim != null)
        {
            Skeleton skeleton = skeletonMecanim.Skeleton;
            Slot slot = skeleton.FindSlot(slotName);

            if (slot != null)
            {
                slot.A = 0; // 투명도 0 → 슬롯 숨김
                Debug.Log($"Slot '{slotNameToHide}' 숨김 처리됨.");
            }
            else
            {
                Debug.LogError($"Slot '{slotNameToHide}'을 찾을 수 없음.");
            }
        }
        else
        {
            Debug.LogError("SkeletonMecanim을 찾을 수 없음.");
        }
    }

    public void MakeWeaponHide()
    {
        for(int i=0; i<weaponName.Length; i++)
        {
            MakeSlotHide(weaponName[i]);
        }
    }

    public void AttachObjectToArm()
    {
        Skeleton skeleton = skeletonMecanim.Skeleton;
        Slot slot = skeleton.FindSlot(armName);

        bone = slot.Bone;

        Debug.Log(gameManager.isHaveKey);
        if(gameManager.isHaveKey == false)
        {
            MoveAttachObject();
        }
    }

    public void MoveAttachObject(bool isHaveKey = false)
    {
        attachObjectPos.GetComponent<Collider>().isTrigger = true;
        if (bone!= null)
        {
            Vector3 localPosition = new Vector3(bone.WorldX, bone.WorldY, 0);
            attachObjectPos.position = skeletonMecanim.transform.TransformPoint(localPosition) + new Vector3(0, 0, -0.05f);
        }
    }

    public void TurnOffPortal()
    {
        gameManager.portalController.transform.parent.gameObject.SetActive(false);
    }

    public void SetUIActive(bool isActive)
    {
        gameManager.SetPopUpActive(isActive);
    }

    public void SetKeyActive(bool isActive)
    {
        gameManager.key.SetActive(isActive);
    }
    #endregion

    public int NormalAttack()
    {
        return normalAttackNum;
    }

    public int ThrowAttack()
    {
        return throwAttackNum;
    }

    public void ClearEnemyNum()
    {
        gameManager.enemyNum++;
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
        if(collision.gameObject.CompareTag("Movable") || collision.gameObject.layer == LayerMask.NameToLayer("Movable"))
        {
            isCanCooperate = true;
            Debug.Log("PlayerMovement.isCanCooperate : " + isCanCooperate);
            isColliderActive = true;
            if(currentAction != ActionState.PlaceObject)
            {
                currentAction = ActionState.Movable;
                objectCol = collision.collider;
                hitColliderObject = collision.collider.gameObject;
                gameManager.SetPopUpUIByType(currentAction);
            }
            else
            {
                gameManager.SetUIPutObject();
            }
            gameManager.SetPopUpActive(true, collision.collider.transform);

            if (currentMode == PlayerMode.Mode_2D && isPickUpAlready == false)
            {
                attachObjectPos = collision.transform;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(isCanCooperate == true && currentAction == ActionState.Movable)
        {
            Debug.Log("Exit");
            currentAction = ActionState.None;
            isCanCooperate = false;
        }

        if(isColliderActive == true)
        {
            isColliderActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            portalCtrl = other.GetComponent<PortalController>();
            isInPortal = true;
            currentAction = ActionState.Portal;
            objectCol = null;
            if(currentMode == PlayerMode.Mode_3D)
            {
                gameManager.SetPopUpUIByType(currentAction, true);
            }
            else
            {
                gameManager.SetPopUpUIByType(currentAction, false);
            }
            gameManager.SetPopUpActive(true, other.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            isInPortal = true;
            currentAction = ActionState.Portal;
            objectCol = null;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Movable"))
        {
            if(currentAction == ActionState.PlaceObject)
            {
                gameManager.SetUIPutObject();
                gameManager.SetPopUpActive(true, other.transform);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(isInPortal == true)
        {
            if (gameManager.isCanvasOpen() == true)
            {
                gameManager.SetPopUpActive(false);
            }
            isInPortal = false;

        }

    }
}
