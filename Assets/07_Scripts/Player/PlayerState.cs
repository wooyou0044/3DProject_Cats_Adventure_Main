using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void EnterState(PlayerMovement player);
    void UpdateState(PlayerMovement player);
    void FixedUpdateState(PlayerMovement player);
}

public class IdleState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetBool("IsRun", false);
    }

    public void UpdateState(PlayerMovement player)
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        if(moveDir != Vector3.zero)
        {
            player.ChangeState(new WalkingState());
        }

        if(Input.GetKeyDown(KeyCode.Space) && player.CanJump())
        {
            player.ChangeState(new JumpingState());
        }


    }

    public void FixedUpdateState(PlayerMovement player)
    {
    }
}

public class WalkingState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        throw new System.NotImplementedException();
    }
    public void UpdateState(PlayerMovement player)
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdateState(PlayerMovement player)
    {
        throw new System.NotImplementedException();
    }
}

public class JumpingState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(PlayerMovement player)
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdateState(PlayerMovement player)
    {
        throw new System.NotImplementedException();
    }
}

// 공격받으면 체력 UI 뜨는 것 추가 필요
