using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChangeState(IPlayerState state)
    {

    }

    public bool CanJump()
    {
        bool isGround = true;

        return isGround;
    }
}
