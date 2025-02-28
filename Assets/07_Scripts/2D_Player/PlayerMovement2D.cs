using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    IPlayer2DState currentState;

    public Rigidbody rigid;
    public Animator animator;

    //public SkeletonMecanim skeletonMecanim;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        ChangeState(new IdleState2D());

        //skeletonMecanim = GetComponent<SkeletonMecanim>();
        //Skeleton skeleton = skeletonMecanim.Skeleton;
        //Slot weaponSlot = skeleton.FindSlot("arm3");
        //if(weaponSlot != null)
        //{
        //    Debug.Log(weaponSlot.Attachment);
        //    weaponSlot.Attachment = null;
        //    Debug.Log(weaponSlot.Attachment);
        //}
        //else
        //{
        //    Debug.Log("¸ø Ã£À½");
        //}
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
}
