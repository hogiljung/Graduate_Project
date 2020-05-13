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

    public SteamVR_Behaviour_Pose controllerPose;

    public GameObject playerCamera;             //머리
    public GameObject isTeleport;               //텔레포트 모드 확인
    public GameObject handle;                   //큐 핸들
    public GameObject cue;                      //큐
    public GameObject menu_obj;                 //메뉴 캔버스
    public GameObject lazer;                    //메뉴 조작 레이저
    public GameObject main;
    public GameObject option;
    public GameObject replay;

    private GameObject collidingObject;
    private GameObject objectInHand;

    private SteamVR_TrackedObject mTrackedObj;  //트래킹하는 오브젝트
    private Teleport mTeleport;                 //텔레포트 스크립트
    private Transform mPlayer;                  //플레이어
    private Mode mmode;                         //현재 상태
    private CueGrap isGrab;                     //고정버튼 눌렸는지

    private Vector3 front;                      //카메라 기준 정면 이동량
    private Vector3 side;                       //카메라 기준 측면 이동량
    private Vector3 lookAt;                     //큐가 바라볼 곳

    private bool isMove;                        //이동버튼 눌렸는지

    public float speed;                         //
    

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = transform.parent.parent;
        mTeleport = FindObjectOfType<Teleport>();
        mTrackedObj = GetComponent<SteamVR_TrackedObject>();
        mmode = FindObjectOfType<Mode>();
        isGrab = FindObjectOfType<CueGrap>();
        speed = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        // 왼손 터치패드 동작

        //옵션으로 모드 조정해서 텔레포트, 방향이동 선택
        if (isTeleport.activeSelf)
            Teleporting();
        else
            Moving();

        MenuAction();       //메뉴
        switch (mmode.mode)
        {
            case 0:     //기본 상태
                GrapAction();       //물건 집기
                break;
            case 1:     //큐 든 상태
                GrapCue();          //큐 고정
                break;
            case 2:     //메뉴 상태

                break;
            case 3:     //물건 든 상태
                GrapAction();       //물건 놓기/던지기
                break;
        }


    }

    // 큐 고정
    private void GrapCue()
    {
        if (graps.GetStateDown(handType))
        {
            Debug.Log("Left graps down");
            isGrab.IsGrap = true;
        }
        if (graps.GetStateUp(handType))
        {
            Debug.Log("Left graps up");
            isGrab.IsGrap = false;
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
            }

        }
        if (PadClick.GetStateUp(handType))
        {
            if (mTeleport)
            {
                mTeleport.mIsActive = false;
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
                Debug.Log("Menu off");
                lazer.SetActive(false);
                mmode.mode = 0;
                menu_obj.SetActive(false);
            }
            else    // 메뉴 추가하면 찾아서 초기화 해주어야함!
            {
                Debug.Log("Menu on");
                cue.SetActive(false);
                isGrab.IsGrap = false;
                menu_obj.SetActive(true);
                main.SetActive(true);
                option.SetActive(false);
                replay.SetActive(false);
                lazer.SetActive(true);
                mmode.mode = 2;
            }
        }
    }

    //물건 들기
    private void GrapAction()
    {
        if (graps.GetStateDown(handType))
        {
            if (collidingObject)
            {
                Grap();
            }
        }
        else if (graps.GetStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            collidingObject = other.gameObject;
        }
    }

    public void OnTriggerStay(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }
    

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void Grap()
    {
        Debug.Log("Grap");
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        mmode.mode = 3;
    }

    private void ReleaseObject()
    {
        Debug.Log("Release");
        GetComponent<FixedJoint>().connectedBody = null;
        Destroy(GetComponent<FixedJoint>());

        objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
        objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        objectInHand = null;
        mmode.mode = 0;
    }

}
