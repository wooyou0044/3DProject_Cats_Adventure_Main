using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas popUpCanvas;
    [SerializeField] GameObject player;
    [SerializeField] Canvas NPCTalkCanvas;
    [SerializeField] GameObject player2D;
    [SerializeField] Transform[] mapTrans;

    public bool isPlayer3D { get; set; }
    public int currentMapNum { get; set; }

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
            popUpCanvas.transform.position = popUpObject.position + new Vector3(0, playerMove.GetHitObjectSizeY() + 1.5f, 0);
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

        popUpUI.SetPopUpText(type);
    }

    public void SetNPCTalkCanvasActive(bool isActive)
    {
        if(isActive == true)
        {
            popUpCanvas.gameObject.SetActive(false);
        }
        NPCTalkCanvas.gameObject.SetActive(isActive);
    }

    public void SetConversationInUI(int clickNum)
    {
        if (conversationUI.isEnd == true)
        {
            //SetNPCTalkCanvasActive(false);
            ReceiveConversationReset();
        }
        if (conversationUI.isAlready == false && playerMove.isPressEnter == true)
        {
            SendEnterPressed();
        }
        if (conversationUI.isAlready == true)
        {
            conversationUI.SetConverstation(clickNum);
        }
    }

    public bool SendConversationEnd()
    {
        return conversationUI.isEnd;
    }

    void ReceiveConversationReset()
    {
        conversationUI.isEnd = false;
        conversationUI.isEnterPressed = false;
    }

    public bool SendMessageAllRepresent()
    {
        return conversationUI.isAlready;
    }

    void SendEnterPressed()
    {
        conversationUI.isEnterPressed = (playerMove.isPressEnter == true)? true : false;
    }

    public void SetUIPutObject()
    {
        popUpUI.SetPopUpText(PlayerMovement.ActionState.PlaceObject);
    }

    public void SetPopUpUIByType(PlayerMovement.ActionState type, bool is3D = true)
    {
        if(type == PlayerMovement.ActionState.Portal)
        {
            popUpUI.ChangeWarpText(is3D);
        }
        popUpUI.SetPopUpText(type);
    }

    public void Make2DPlayer(int currentMapNum)
    {
        // 한 번만 만들도록 조건식 필요
        Instantiate(player2D, mapTrans[currentMapNum].position + new Vector3(0,-2,-0.01f), Quaternion.identity, mapTrans[currentMapNum]);

    }
}
