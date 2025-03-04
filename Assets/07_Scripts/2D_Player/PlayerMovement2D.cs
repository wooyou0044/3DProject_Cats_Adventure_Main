using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpHeight = 5.0f;
    IPlayer2DState currentState;

    public Rigidbody rigid;
    public Animator animator;

    public bool isJumpingFloor { get; set; }

    SkeletonMecanim skeletonMecanim;
    [SpineSlot] public string slotNameToHide;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        skeletonMecanim = animator.GetComponent<SkeletonMecanim>();
    }

    void Start()
    {
        isJumpingFloor = true;
        //ChangeState(new IdleState2D());

        MakeSlotHide(slotNameToHide);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void ChangeState(IPlayer2DState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    
    public void RemoveShield()
    {

    }

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
                slot.A = 0; // Åõ¸íµµ 0 ¡æ ½½·Ô ¼û±è
                Debug.Log($"Slot '{slotNameToHide}' ¼û±è Ã³¸®µÊ.");
            }
            else
            {
                Debug.LogError($"Slot '{slotNameToHide}'À» Ã£À» ¼ö ¾øÀ½.");
            }
        }
        else
        {
            Debug.LogError("SkeletonMecanimÀ» Ã£À» ¼ö ¾øÀ½.");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            isJumpingFloor = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Movable"))
        {
            Debug.Log("Æ®·¥ÆÞ¸° ºÎµúÈû");
        }
    }
}
