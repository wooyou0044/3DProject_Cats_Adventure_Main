using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour
{
    public enum LayerType
    {
        None,
        NPC,
        Movable,
        Portal,
        Wire
    }

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

    public void SetPopUpText(LayerType type)
    {
        switch (type)
        {
            case LayerType.NPC:
                Debug.Log("NPC");
                popUpText.fontSize = 50;
                // �۵�
                popUpText.text = "�۵�";
                if (popUpImage.gameObject.activeSelf == false)
                {
                    popUpImage.gameObject.SetActive(true);
                }
                break;
            case LayerType.Movable:
                Debug.Log("Movable");
                popUpText.fontSize = 50;
                // 2D - �ݱ�
                // ���
                popUpText.text = "���";
                if (popUpImage.gameObject.activeSelf == false)
                {
                    popUpImage.gameObject.SetActive(true);
                }
                break;
            case LayerType.Portal:
                popUpText.fontSize = 45;
                // ���� - ������
                break;
            case LayerType.Wire:
                // ���ڰ� �� ��
                break;
        }
    }
}
