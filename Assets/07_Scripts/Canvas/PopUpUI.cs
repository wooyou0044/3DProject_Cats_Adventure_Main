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
                // �۵�
                popUpText.text = "�۵�";
                if (popUpImage.gameObject.activeSelf == false)
                {
                    popUpImage.gameObject.SetActive(true);
                }
                break;
            case PlayerMovement.ActionState.Movable:
                popUpText.fontSize = 40;
                // 2D - �ݱ�
                // ���
                popUpText.text = "���";
                if (popUpImage.gameObject.activeSelf == false)
                {
                    popUpImage.gameObject.SetActive(true);
                }
                break;
            case PlayerMovement.ActionState.PlaceObject:
                popUpText.text = "����";
                break;
            case PlayerMovement.ActionState.Portal:
                popUpText.fontSize = 35;
                // ���� - ������
                //popUpText.text = "����";
                break;
            case PlayerMovement.ActionState.Wire:
                // ���ڰ� �� ��
                break;
        }
    }

    public void ChangeWarpText(bool is3D)
    {
        if(is3D == true)
        {
            popUpText.text = "����";
        }
        else
        {
            popUpText.text = "������";
        }
    }
}
