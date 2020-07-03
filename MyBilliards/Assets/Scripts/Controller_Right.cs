using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller_Right : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grapAction;
    public SteamVR_Action_Boolean forword;
    public SteamVR_Action_Boolean backword;
    public SteamVR_Action_Boolean left;
    public SteamVR_Action_Boolean right;
    public SteamVR_Action_Boolean menu;

    public SteamVR_Behaviour_Pose controllerPose;

    public GameObject isTeleport;
    public GameObject cue;
    public GameObject handle;
    public GameObject menu_obj;
    public GameObject lazer;
    public GameObject main;
    public GameObject option;
    public GameObject replay;
    public GameObject hand_normal;
    public GameObject hand_fist;
    public Transform camdir;

    public Transform holdPosition;              //큐 고정 위치

    private Collider cueCol;
    private Rigidbody mPlayer;                  //플레이어
    private RaycastHit hit;

    private GameObject collidingObject;
    private GameObject objectInHand;
    private Vector3 vHoldPos;
    private Vector3 controllerMovePos;
    private float cuepos;
    private float theta;
    private float degree;
    private SendData sd;

    private Mode mmode;
    private CueGrap isGrap;

    private bool cuegrap;
    private bool isJump;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = transform.parent.parent.GetComponent<Rigidbody>();
        cueCol = cue.GetComponent<Collider>();
        mmode = FindObjectOfType<Mode>();
        isGrap = FindObjectOfType<CueGrap>();
        sd = FindObjectOfType<SendData>();
        cuegrap = false;
        isJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        PadAction();
        MenuAction();

        switch (mmode.mode)
        {
            case 0:     // 기본 상태
                CueAction();        //큐 꺼내기
                GrapAction();       //물건 집기
                break;
            case 1:     // 큐 든 상태
                CueAction();        //큐 넣기
                Follow();           //큐 위치 지정
                StartREC();
                break;
            case 2:     // 메뉴 상태
                
                break;
            case 3:     // 물건 든 상태
                GrapAction();       //물건 놓기/던지기
                break;
        }
    }

    private void StartREC()
    {

        if (grapAction.GetStateDown(handType))
        {
            if(!sd.startREC)
                sd.startREC = true;
        }

    }

    //컨트롤러 이벤트
    private void PadAction()
    {
        if (forword.GetStateDown(handType))
        {
            if (!isJump)
            {
                isJump = true;
                StartCoroutine("Jump");
            }
        }
        if (left.GetStateDown(handType))
        {
            camdir.Rotate(0, 15, 0);
        }
        if (right.GetStateDown(handType))
        {
            camdir.Rotate(0, -15, 0);
        }
    }

    IEnumerator Jump()
    {
        //Debug.Log("jump");
        mPlayer.AddForce(0, 350, 0);
        yield return new WaitForSeconds(0.8f);
        isJump = false;
    }

    //큐 들기
    private void CueAction()
    {
        if (backword.GetStateDown(handType))        //터치패드 아래버튼
        {
            if (!cue.activeSelf)        //큐가 꺼져있으면
            {
                //Debug.Log("cue on");
                mmode.mode = 1;
                cue.SetActive(true);
                hand_normal.SetActive(false);
            }
            else                        //큐가 켜져있으면
            {
                //Debug.Log("cue off");
                mmode.mode = 0;
                cue.SetActive(false);
                hand_normal.SetActive(true);
            }
        }
    }

    //큐 손에 든것처럼 따라다니기
    private void Follow()
    {
        switch (PlayerPrefs.GetInt("assist", 0))
        {
            case 0:     //어시스트모드 off
                handle.transform.position = transform.position;
                if (isGrap.IsGrap)
                {
                    if(!cueCol.isTrigger)
                        cueCol.isTrigger = true;
                    handle.transform.LookAt(holdPosition);
                }
                else
                {
                    if (cueCol.isTrigger)
                        cueCol.isTrigger = false;
                    if (cuegrap)
                        cuegrap = false;
                    handle.transform.rotation = transform.rotation;
                    vHoldPos = handle.transform.position;
                }
                break;

            case 1:     //어시스트모드 on
                if (isGrap.IsGrap)
                {
                    if (!cueCol.isTrigger)
                        cueCol.isTrigger = true;
                    controllerMovePos = controllerPose.transform.position - vHoldPos;
                    theta = Vector3.Dot(handle.transform.forward, controllerMovePos);
                    degree = Mathf.Rad2Deg * theta;
                    //Debug.Log("각도: " + degree);
                    cuepos = (controllerPose.transform.position - vHoldPos).magnitude * degree * 0.08f;
                    handle.transform.position = vHoldPos + cuepos * (handle.transform.forward);
                }
                else
                {
                    Physics.Raycast(cue.transform.position, cue.transform.forward, out hit, 10f);
                    handle.transform.position = transform.position;
                    handle.transform.rotation = transform.rotation;
                    vHoldPos = handle.transform.position;
                    if (cuegrap)
                        cuegrap = false;
                    if (cueCol.isTrigger)
                        cueCol.isTrigger = false;
                }
                break;
        }
    }

    //물건 들기
    private void GrapAction()
    {
        if (grapAction.GetStateDown(handType))
        {
            if (hand_normal.activeSelf)
            {
                hand_normal.SetActive(false);
                hand_fist.SetActive(true);
            }
            if (collidingObject != null)
            {
                Grap();
            }
        }
        else if (grapAction.GetStateUp(handType))
        {
            if (!hand_normal.activeSelf)
            {
                hand_normal.SetActive(true);
                hand_fist.SetActive(false);
            }
            if (objectInHand != null)
            {
                ReleaseObject();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }

    private void SetCollidingObject(Collider col)
    {
        if (col.gameObject.layer  == 12)        //오브젝트일때
        {
            if (col.GetComponent<Rigidbody>())
            {
                collidingObject = col.gameObject; //잡을 수 있는 오브젝트로 입력
            }
        }
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
        //Debug.Log("Grap");
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        mmode.mode = 3;
    }

    private void ReleaseObject()
    {
        if (!objectInHand)
        {
            return;
        }
        if (GetComponent<FixedJoint>())
        {
            //Debug.Log("Release");
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
        mmode.mode = 0;
    }

    //메뉴
    private void MenuAction()
    {
        if (menu.GetStateDown(handType))
        {
            if (menu_obj.activeSelf)
            {
                //Debug.Log("Menu off");
                lazer.SetActive(false);
                mmode.mode = 0;
                menu_obj.SetActive(false);
            }
            else    // 메뉴 추가하면 찾아서 초기화 해주어야함!
            {
                //Debug.Log("Menu on");
                hand_fist.SetActive(false);
                hand_normal.SetActive(true);
                menu_obj.SetActive(true);
                main.SetActive(true);
                option.SetActive(false);
                replay.SetActive(false);
                lazer.SetActive(true);
                mmode.mode = 2;
                isGrap.IsGrap = false;
                cue.SetActive(false);
            }
        }
    }

}
