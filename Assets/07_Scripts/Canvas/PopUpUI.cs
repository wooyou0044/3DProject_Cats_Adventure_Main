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
                // 작동
                popUpText.text = "작동";
                if (popUpImage.gameObject.activeSelf == false)
                {
                    popUpImage.gameObject.SetActive(true);
                }
                break;
            case LayerType.Movable:
                Debug.Log("Movable");
                popUpText.fontSize = 50;
                // 2D - 줍기
                // 잡기
                popUpText.text = "잡기";
                if (popUpImage.gameObject.activeSelf == false)
                {
                    popUpImage.gameObject.SetActive(true);
                }
                break;
            case LayerType.Portal:
                popUpText.fontSize = 45;
                // 들어가기 - 나가기
                break;
            case LayerType.Wire:
                // 글자가 안 뜸
                break;
        }
    }
}
