using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas popUpCanvas;

    PopUpUI popUpUI;

    private void Awake()
    {
        popUpUI = popUpCanvas.GetComponent<PopUpUI>();
    }

    void Start()
    {
        popUpCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void SetPopUpActive(bool isActive, Transform popUpObject = null)
    {
        Debug.Log(popUpObject.position);
        if (isActive == true && popUpObject != null)
        {
            popUpCanvas.transform.position = popUpObject.position + new Vector3(0, 3.5f, 0);
        }
        popUpCanvas.gameObject.SetActive(isActive);
    }

    public void SetPopUpUIType(int layerNum)
    {
        Debug.Log("NPC : " + LayerMask.GetMask("NPC"));
        PopUpUI.LayerType type = PopUpUI.LayerType.None;
        if (layerNum == LayerMask.GetMask("NPC"))
        {
            type = PopUpUI.LayerType.NPC;
        }

        if(layerNum == LayerMask.GetMask("Movable"))
        {
            type = PopUpUI.LayerType.Movable;
        }
        Debug.Log("type : " + type);
        popUpUI.SetPopUpText(type);
    }
}
