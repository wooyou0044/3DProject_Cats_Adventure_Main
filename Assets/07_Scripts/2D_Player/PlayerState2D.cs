using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer2DState
{
    void EnterState(PlayerMovement2D player);
    void UpdateState(PlayerMovement2D player);
    void FixedUpdateState(PlayerMovement2D player);
}

public class IdleState2D : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetBool("Running", false);
        player.animator.SetBool("Attacking", false);
    }

    public void UpdateState(PlayerMovement player)
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if(v != 0 || h!=0)
        {
            player.ChangeState(new RunState2D());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.ChangeState(new AttackState2D());
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log(player.isCanCooperate);
            if(player.isCanCooperate)
            {
                // �ݴ� ��
                player.ChangeState(new PickUpState2D());
            }
            // ���� �̵�
            if (player.isInPortal == true)
            {
                player.animator.SetTrigger("MoveToPortal");
                player.MoveIntoOtherWorld(false);
                player.isInPortal = false;
            }

            // ���̾� �̵� 
        }

        if(Input.GetKeyDown(KeyCode.Space) && player.isFloor)
        {
            player.ChangeState(new JumpState2D());
        }
    }

    public void FixedUpdateState(PlayerMovement player)
    {
        //player.rigid.velocity = new Vector2(0, 0);
    }
}

public class RunState2D : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetBool("Running", true);
    }

    public void UpdateState(PlayerMovement player)
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // ������ ȸ��
        if (h > 0)
        {
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // ���� ȸ��
        else if(h < 0)
        {
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if(h == 0 || v == 0)
        {
            player.ChangeState(new IdleState2D());
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            player.ChangeState(new AttackState2D());
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            // �ݴ� ��
            if (player.isCanCooperate)
            {
                // �ݴ� ��
                player.ChangeState(new PickUpState2D());
            }
            // ���� �̵�
            if (player.isInPortal == true)
            {
                player.animator.SetTrigger("MoveToPortal");
                player.MoveIntoOtherWorld(false);
                player.isInPortal = false;
            }
            // ���� �̵�

            // ���̾� �̵� 
        }

        if (player.isPickUpAlready == true)
        {
            player.MoveAttachObject();
        }
    }

    public void FixedUpdateState(PlayerMovement player)
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 moveDir = (Vector3.up * v) + (Vector3.right * h);
        player.rigid.velocity = new Vector2(h * player.moveSpeed, v * player.moveSpeed);
    }
}

public class AttackState2D : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetBool("Attacking", true);
    }

    public void UpdateState(PlayerMovement player)
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (v != 0 || h != 0)
        {
            player.ChangeState(new RunState2D());
        }

        if(v == 0 || h ==0)
        {
            player.ChangeState(new IdleState2D());
        }

    }

    public void FixedUpdateState(PlayerMovement player)
    {
        player.rigid.velocity = new Vector2(0, 0);
    }
}

public class JumpState2D : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        player.animator.SetTrigger("Jumping");
        player.rigid.AddForce(Vector2.up * player.jumpHeight, ForceMode.Impulse);
        player.StopJumping();
    }

    public void UpdateState(PlayerMovement player)
    {
        if (player.rigid.velocity.y <= 0)
        {
            player.isFloor = false;
            player.ChangeState(new IdleState2D());
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

public class PickUpState2D : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        //player.animator.SetBool("PickUp", true);
        player.MakeWeaponHide();
        player.AttachObjectToArm();
        player.SetUIActive(false);
        player.isPickUpAlready = true;
        player.ChangeState(new IdleState2D());
    }

    public void UpdateState(PlayerMovement player)
    {

    }

    public void FixedUpdateState(PlayerMovement player)
    {
        player.MoveAttachObject();
    }
}

public class WireMove2D : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
    }

    public void UpdateState(PlayerMovement player)
    {
    }

    public void FixedUpdateState(PlayerMovement player)
    {
    }
}
