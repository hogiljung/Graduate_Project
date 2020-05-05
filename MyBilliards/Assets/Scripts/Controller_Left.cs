using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller_Left : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean graps;        //트리거버튼
    public SteamVR_Action_Boolean PadClick;     //패드 클릭
    public SteamVR_Action_Boolean menu;         //메뉴
    public SteamVR_Action_Boolean TouchPad;     //패드 터치
    public SteamVR_Action_Vector2 TouchPos;     //패드 터치좌표

    public GameObject playerCamera;             //머리
    public GameObject TeleportArea;             //텔레포트 영역 표시
    public GameObject isTeleport;               //텔레포트 모드 확인
    public GameObject handle;                   //큐대 핸들
    public GameObject menu_obj;                 //메뉴 캔버스
    public GameObject lazer;                    //메뉴 조작 레이저

    private SteamVR_TrackedObject mTrackedObj;  //트래킹하는 오브젝트
    private Teleport mTeleport;                 //텔레포트 스크립트
    private Transform mPlayer;                  //플레이어
    private Mode mmode;                         //현재 상태

    private Vector3 front;                      //카메라 기준 정면 이동량
    private Vector3 side;                       //카메라 기준 측면 이동량
    private Vector3 lookAt;                     //큐가 바라볼 곳

    private bool isMove;                        //이동버튼 눌렸는지
    private bool isGrab;                        //고정버튼 눌렸는지

    public float speed;                         //
    

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = transform.parent;
        mTeleport = FindObjectOfType<Teleport>();
        mTrackedObj = GetComponent<SteamVR_TrackedObject>();
        mmode = FindObjectOfType<Mode>();
        speed = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        // 왼손 터치패드 동작
        if (!isGrab)        // 큐 조준시에는 이동불가
        {
            if (isTeleport.activeSelf)       //옵션으로 모드 조정해서 텔레포트, 방향이동 선택
                Teleporting();
            else
                Moving();
        }

        switch (mmode.mode)
        {
            case 0:     //기본 상태
                MenuAction();
                break;
            case 1:     //큐 든 상태
                //GrapCue();
                GrapCue();
                break;
            case 2:     //메뉴 상태
                MenuAction();
                break;
            case 3:     //물건 든 상태

                break;
        }
        
        
    }
    /*
    //큐 고정
    private void GrapCue()
    {
        if (isGrab)     //잡은 상태에선 잡기 누른 포지션을 큐가 바라보도록 설정
        {
            handle.transform.LookAt(lookAt);
        }
        else            //놓은 상태에서 
        {
            handle.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
    }*/

    IEnumerator CGrapCue()
    {
        while (isGrab)
        {
            handle.transform.LookAt(lookAt);
            yield return new WaitForSeconds(0.011f);
        }
        handle.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void GrapCue()
    {
        if (graps.GetStateDown(handType))
        {
            if (!isGrab)
            {
                lookAt = transform.position;
            }
            Debug.Log("Left graps down");
            isGrab = true;
            StartCoroutine("CGrapCue");
        }
        if (graps.GetStateUp(handType))
        {
            Debug.Log("Left graps up");
            isGrab = false;
        }

    }

    //방향키 이동
    private void Moving()
    {
        
        if (PadClick.GetStateDown(handType))
        {
            Debug.Log("move button down");
            isMove = true;
        }
        if (TouchPad.GetStateUp(handType))
        {
            Debug.Log("move button up");
            isMove = false;
        }
        if (isMove)
        {
            front = playerCamera.transform.forward * TouchPos.GetAxis(handType).y;
            side = playerCamera.transform.right * TouchPos.GetAxis(handType).x;
            //Debug.Log(TouchPos.GetAxis(handType) + "  " + playerCamera.transform.forward + "  " + playerCamera.transform.right);
            mPlayer.transform.Translate((front.x + side.x) * speed, 0, (front.z + side.z) * speed);
        }
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

    private void MenuAction()
    {
        if (menu.GetStateDown(handType))
        {
            if (menu_obj.activeSelf)
            {
                lazer.SetActive(false);
                menu_obj.SetActive(false);
                mmode.mode = 0;
            }
            else
            {
                mmode.mode = 2;
                menu_obj.SetActive(true);
                lazer.SetActive(true);
            }
        }
    }
}
