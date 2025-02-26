using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

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
        //player.animator.SetBool("IsRun", false);
        player.animator.SetBool("Talking", false);
        player.animator.SetBool("MovingStuff", false);
    }

    public void UpdateState(PlayerMovement player)
    {
        //float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        float moveMagnitude = Mathf.Clamp01(Mathf.Abs(v));
        player.animator.SetFloat("MoveFloat", moveMagnitude);

        //Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h).normalized;
        Vector3 moveDir = (Vector3.forward * v).normalized;

        if(r != 0)
        {
            player.transform.Rotate(Vector3.up * player.turnSpeed * Time.deltaTime * r);
        }

        if(moveDir.z > 0)
        {
            player.ChangeState(new RunningState());
        }

        if(Input.GetKeyDown(KeyCode.Space) && player.CanJump())
        {
            player.ChangeState(new JumpingState());
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            player.ChangeState(new AttackState());
        }

        if(Input.GetMouseButtonDown(0) && player.CanThrow())
        {
            player.ChangeState(new ThrowWeaponState());
        }

        if(player.isCanCooperate == true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                player.ChangeState(new DoCooperateState());
                // NPC와 대화
                // 물건 밀기
                // 물건 잡기
                // 워프 안에 들어가기
                // 물건 놓기/던지기
            }
        }
    }

    public void FixedUpdateState(PlayerMovement player)
    {
        if(player.isColliderActive == false)
        {
            player.DoEyeSight();
        }
        // 무기를 던졌다면
        if (player.isThrow)
        {
            player.ReturnWeaponInHand();
        }
    }
}

public class RunningState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetBool("Talking", false);
        player.animator.SetBool("MovingStuff", false);
    }
    public void UpdateState(PlayerMovement player)
    {
        //float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        float moveMagnitude = Mathf.Clamp01(Mathf.Abs(v));
        player.animator.SetFloat("MoveFloat", moveMagnitude);

        if (r != 0)
        {
            player.transform.Rotate(Vector3.up * player.turnSpeed * Time.deltaTime * r);
        }

        if (v == 0)
        {
            player.ChangeState(new IdleState());
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.CanJump())
        {
            player.ChangeState(new JumpingState());
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            player.ChangeState(new AttackState());
        }

        if(Input.GetMouseButtonDown(0))
        {
            player.ChangeState(new ThrowWeaponState());
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(player.isCanCooperate == true)
            {
                player.ChangeState(new DoCooperateState());
                // NPC와 대화
                // 물건 밀기
                // 물건 잡기
                // 워프 안에 들어가기
                // 물건 놓기/던지기
            }
        }
    }

    public void FixedUpdateState(PlayerMovement player)
    {
        if (player.isThrow)
        {
            player.ReturnWeaponInHand();
        }

        float v = Input.GetAxis("Vertical");
        Vector3 moveDir = (Vector3.forward * v);

        player.transform.Translate(moveDir.normalized * player.moveSpeed * Time.deltaTime);

        if(player.isColliderActive == false)
        {
            player.DoEyeSight();
        }
    }
}

public class JumpingState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetTrigger("Jump");
        player.rigid.AddForce(Vector3.up * player.jumpHeight, ForceMode.Impulse);
    }

    public void UpdateState(PlayerMovement player)
    {
        if(player.rigid.velocity.y <= 0)
        {
            player.ChangeState(new IdleState());
        }
    }

    public void FixedUpdateState(PlayerMovement player)
    {
        if (player.isFloor == true)
        {
            player.isFloor = false;
        }
    }
}

public class AttackState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetTrigger("NormalAttack");
    }

    public void UpdateState(PlayerMovement player)
    {
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        if(v != 0)
        {
            player.ChangeState(new RunningState());
        }
        else
        {
            player.ChangeState(new IdleState());
        }
    }

    public void FixedUpdateState(PlayerMovement player)
    {
    }
}

public class ThrowWeaponState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetTrigger("ThrowWeapon");
        player.ThrowWeaponAttack();
    }

    public void UpdateState(PlayerMovement player)
    {
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        if (v != 0)
        {
            player.ChangeState(new RunningState());
        }
        else
        {
            player.ChangeState(new IdleState());
        }
    }

    public void FixedUpdateState(PlayerMovement player)
    {
    }
}

public class DoCooperateState : IPlayerState
{
    int spaceNum;

    public void EnterState(PlayerMovement player)
    {
        player.ChangeYAnimation();
        if (player.currentAction == ActionState.NPC)
        {
            player.ConversationWithNPC(spaceNum);
        }
    }

    public void UpdateState(PlayerMovement player)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            spaceNum++;
            if (player.currentAction == ActionState.NPC)
            {
                player.ConversationWithNPC(spaceNum);
            }
        }
    }

    public void FixedUpdateState(PlayerMovement player)
    {
    }
}

// 공격받으면 체력 UI 뜨는 것 추가 필요
