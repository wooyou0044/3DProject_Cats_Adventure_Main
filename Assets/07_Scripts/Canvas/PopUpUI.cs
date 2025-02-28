using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour
{
    [SerializeField] Image popUpImage;
    [SerializeField] Text popUpText;
    [SerializeField] Image popUpWireImage;

    void Start()
    {
        popUpImage.gameObject.SetActive(false);
        popUpWireImage.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void SetPopUpText(PlayerMovement.ActionState type)
    {
        switch (type)
        {
            case PlayerMovement.ActionState.NPC:
                popUpText.fontSize = 45;
                // 작동
                popUpText.text = "작동";
                if (popUpImage.gameObject.activeSelf == false)
                {
                    popUpImage.gameObject.SetActive(true);
                }
                break;
            case PlayerMovement.ActionState.Movable:
                popUpText.fontSize = 40;
                // 2D - 줍기
                // 잡기
                popUpText.text = "잡기";
                if (popUpImage.gameObject.activeSelf == false)
                {
                    popUpImage.gameObject.SetActive(true);
                }
                break;
            case PlayerMovement.ActionState.PlaceObject:
                popUpText.text = "놓기";
                break;
            case PlayerMovement.ActionState.Portal:
                popUpText.fontSize = 35;
                // 들어가기 - 나가기
                //popUpText.text = "들어가기";
                break;
            case PlayerMovement.ActionState.Wire:
                // 글자가 안 뜸
                break;
        }
    }

    public void ChangeWarpText(bool is3D)
    {
        if(is3D == true)
        {
            popUpText.text = "들어가기";
        }
        else
        {
            popUpText.text = "나가기";
        }
    }
}
