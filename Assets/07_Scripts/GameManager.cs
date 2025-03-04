using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas popUpCanvas;
    [SerializeField] GameObject player;
    [SerializeField] Canvas NPCTalkCanvas;
    [SerializeField] GameObject player2D;
    [SerializeField] Transform[] mapTrans;

    [SerializeField] GameObject trampolineObject;
    [SerializeField] GameObject keyObject;
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] CinemachineVirtualCamera virtualCam2D;

    public bool isHaveKey { get; set; }
    public int currentMapNum { get; set; }
    public PickUpObjectType pickUpType { get; set; }
    public GameObject key { get; set; }
    public int enemyNum { get; set; }

    GameObject overPortalObject;

    PopUpUI popUpUI;
    ConversationUI conversationUI;
    PlayerMovement playerMove;
    public PortalController portalController { get; private set; }

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

    public void Make2DPlayer()
    {
        portalController = playerMove.portalCtrl;
        // 한 번만 만들도록 조건식 필요
        GameObject player = Instantiate(player2D, mapTrans[currentMapNum].position + new Vector3(0,-2,-0.38f), Quaternion.identity, mapTrans[currentMapNum]);
        virtualCam.gameObject.SetActive(false);
        virtualCam2D.Follow = player.transform;
        //Instantiate(player2D, mapPortalPos.position + new Vector3(0,-2,-0.01f), Quaternion.identity, mapTrans[currentMapNum]);
        if(isHaveKey == true)
        {
            playerMove = player.GetComponent<PlayerMovement>();
            playerMove.MakeWeaponHide();
            GameObject object2D = keyObject.GetComponent<ObjectController>().objectType.pick2DObject;
            key = Instantiate(object2D, mapTrans[currentMapNum].position + new Vector3(0, -2, -0.01f), Quaternion.identity, mapTrans[currentMapNum]);
            key.GetComponent<Collider>().isTrigger = true;
            playerMove.attachObjectPos = key.transform;
            playerMove.AttachObjectToArm();
            playerMove.isPickUpAlready = true;
            //isHaveKey = false;
        }
    }

    public void Set3DObjectActive(bool isActive)
    {
        player.SetActive(isActive);
        if (pickUpType == PickUpObjectType.Trampoline)
        {
            overPortalObject = Instantiate(trampolineObject);
            isHaveKey = false;
        }
        else
        {
            overPortalObject = Instantiate(keyObject);
            isHaveKey = true;
        }
        virtualCam.gameObject.SetActive(true);
        playerMove.isComplete = true;
        playerMove.MakePickUpObject(overPortalObject, true);
        playerMove.isPickUpAlready = true;
        playerMove.isCanCooperate = true;
        playerMove.ChangeState(new MoveObjectState());
    }
}
