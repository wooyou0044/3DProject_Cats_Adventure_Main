using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer2DState
{
    public void EnterState(PlayerMovement2D player);
    public void UpdateState(PlayerMovement2D player);
    public void FixedUpdateState(PlayerMovement2D player);
}

public class IdleState2D : IPlayer2DState
{
    public void EnterState(PlayerMovement2D player)
    {
        player.RemoveShield();
        player.animator.SetBool("Running", false);
        player.animator.SetBool("Attacking", false);
    }

    public void UpdateState(PlayerMovement2D player)
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
            // 줍는 것
            //player.ChangeState(new PickUpState2D());
            // 워프 이동

            // 와이어 이동 
        }
    }

    public void FixedUpdateState(PlayerMovement2D player)
    {

    }
}

public class RunState2D : IPlayer2DState
{
    public void EnterState(PlayerMovement2D player)
    {
        player.RemoveShield();
        player.animator.SetBool("Running", true);
    }

    public void UpdateState(PlayerMovement2D player)
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        // 오른쪽 회전
        if (h > 0)
        {
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // 왼쪽 회전
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
            // 줍는 것
            //player.ChangeState(new PickUpState2D());
            // 워프 이동

            // 와이어 이동 
        }
    }

    public void FixedUpdateState(PlayerMovement2D player)
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 moveDir = ((Vector3.up * v) + (Vector3.right * h));
        //player.transform.Translate(moveDir * player.moveSpeed * Time.deltaTime);
        player.rigid.velocity = new Vector2(h * player.moveSpeed, v * player.moveSpeed);
    }
}

public class AttackState2D : IPlayer2DState
{
    public void EnterState(PlayerMovement2D player)
    {
        player.RemoveShield();
        player.animator.SetBool("Attacking", true);
    }

    public void UpdateState(PlayerMovement2D player)
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

    public void FixedUpdateState(PlayerMovement2D player)
    {
    }
}

public class PickUpState2D : IPlayer2DState
{
    public void EnterState(PlayerMovement2D player)
    {
    }

    public void UpdateState(PlayerMovement2D player)
    {
    }

    public void FixedUpdateState(PlayerMovement2D player)
    {
    }
}

public class WireMove2D : IPlayer2DState
{
    public void EnterState(PlayerMovement2D player)
    {
    }

    public void UpdateState(PlayerMovement2D player)
    {
    }

    public void FixedUpdateState(PlayerMovement2D player)
    {
    }
}
