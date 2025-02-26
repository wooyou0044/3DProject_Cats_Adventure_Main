using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas popUpCanvas;
    [SerializeField] GameObject player;
    [SerializeField] Canvas NPCTalkCanvas;

    PopUpUI popUpUI;
    ConversationUI conversationUI;
    PlayerMovement playerMove;

    private void Awake()
    {
        popUpUI = popUpCanvas.GetComponent<PopUpUI>();
        conversationUI = NPCTalkCanvas.GetComponent<ConversationUI>();
        playerMove = player.GetComponent<PlayerMovement>();
    }

    void Start()
    {
        popUpCanvas.gameObject.SetActive(false);
        NPCTalkCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void SetPopUpActive(bool isActive, Transform popUpObject = null)
    {
        if (isActive == true && popUpObject != null)
        {
            if(playerMove.SetHitObject().layer == LayerMask.NameToLayer("NPC"))
            {
                popUpCanvas.transform.position = popUpObject.position + new Vector3(0, 2.5f, 0);
            }
            else
            {
                popUpCanvas.transform.position = popUpObject.position + new Vector3(0, 3.5f, 0);
            }
        }
        if(isActive == true)
        {
            popUpCanvas.gameObject.SetActive(isActive);
        }
        else
        {
            StartCoroutine(OffCanvas());
        }
    }

    IEnumerator OffCanvas()
    {
        yield return new WaitForSeconds(0.5f);
        popUpCanvas.gameObject.SetActive(false);
    }

    public bool isCanvasOpen()
    {
        return popUpCanvas.gameObject.activeSelf;
    }

    public void SetPopUpUIType(int layerNum)
    {
        PlayerMovement.ActionState type = PlayerMovement.ActionState.None;
        if (layerNum == LayerMask.GetMask("NPC"))
        {
            type = PlayerMovement.ActionState.NPC;
        }

        if(layerNum == LayerMask.GetMask("Movable"))
        {
            type = PlayerMovement.ActionState.Movable;
        }

        popUpUI.SetPopUpText(type);
    }

    public void SetNPCTalkCanvasActive(bool isActive)
    {
        NPCTalkCanvas.gameObject.SetActive(isActive);
    }

    public void SetConversationInUI(int clickNum)
    {
        if(NPCTalkCanvas.gameObject.activeSelf== true)
        {
            conversationUI.SetConverstation(clickNum);
        }
    }
}
