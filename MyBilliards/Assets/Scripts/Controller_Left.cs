using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller_Left : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean graps;
    public SteamVR_Action_Boolean forword;
    public SteamVR_Action_Boolean backword;
    public SteamVR_Action_Boolean left;
    public SteamVR_Action_Boolean right;
    public SteamVR_Action_Boolean TouchPad;
    public SteamVR_Action_Vector2 TouchPos;
    public SteamVR_Action_Boolean PadClick;

    public GameObject player;
    public GameObject playerCamera;
    public GameObject TeleportArea;
    public GameObject isTeleport;

    private SteamVR_TrackedObject mTrackedObj;
    private Teleport mTeleport;
    private Transform mPlayer;

    private Vector3 front;
    private Vector3 side; 
    private float speed;
    private bool isMove;
    // Start is called before the first frame update
    void Start()
    {
        mPlayer = transform.parent;
        mTeleport = FindObjectOfType<Teleport>();
        mTrackedObj = GetComponent<SteamVR_TrackedObject>();
        speed = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        // 왼손 터치패드 동작
        if (isTeleport.activeSelf)       //옵션으로 모드 조정해서 텔레포트, 방향이동 선택
            Teleporting();
        else
            Moving();
    }

    private void GrapCue()
    {
        if (graps.GetState(handType))
        {
            Debug.Log("Left graps");

        }
    }

    private void Moving()
    {
        
        if (PadClick.GetStateDown(handType))
        {
            Debug.Log("move button down");
            isMove = true;
        }
        if (PadClick.GetStateUp(handType))
        {
            Debug.Log("move button up");
            isMove = false;
        }
        if (isMove)
        {
            front = playerCamera.transform.forward * TouchPos.GetAxis(handType).y;
            side = playerCamera.transform.right * TouchPos.GetAxis(handType).x;
            Debug.Log(TouchPos.GetAxis(handType) + "  " + playerCamera.transform.forward + "  " + playerCamera.transform.right);
            player.transform.Translate((front.x + side.x) * speed, 0, (front.z + side.z) * speed);
        }
        /*
        if (GetForwordDown())
        {
            Debug.Log("forword");
            player.transform.Translate(playerCamera.transform.forward.normalized.x * speed, 0, playerCamera.transform.forward.normalized.z * speed);
        }

        if (GetBackwordDown())
        {
            Debug.Log("backword");
            player.transform.Translate(-playerCamera.transform.forward.normalized.x * speed, 0, -playerCamera.transform.forward.normalized.z * speed);
        }

        if (GetLeftDown())
        {
            Debug.Log("left");
            player.transform.Rotate(0, -15, 0);
        }

        if (GetRightDown())
        {
            Debug.Log("right");
            player.transform.Rotate(0, 15, 0);
        }
        */
    }

    private void Teleporting()
    {
        if (PadClick.GetStateDown(handType))
        {
            if (mTeleport)
            {
                mTeleport.mIsActive = true;
                TeleportArea.SetActive(true);
            }

        }
        if (PadClick.GetStateUp(handType))
        {
            if (mTeleport)
            {
                mTeleport.mIsActive = false;
                TeleportArea.SetActive(false);
                Vector3 pos = mTeleport.mGroundPos;
                if (pos != Vector3.zero)
                    mPlayer.transform.position = pos;
            }
        }
        
    }

    private bool GetForwordDown()
    {
        return forword.GetStateDown(handType);
    }

    private bool GetBackwordDown()
    {
        return backword.GetStateDown(handType);
    }

    private bool GetLeftDown()
    {
        return left.GetStateDown(handType);
    }

    private bool GetRightDown()
    {
        return right.GetStateDown(handType);
    }
}
