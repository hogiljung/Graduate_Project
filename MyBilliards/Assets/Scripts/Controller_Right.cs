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
    public SteamVR_Action_Vibration haptic;

    public SteamVR_Behaviour_Pose controllerPose;

    public GameObject isTeleport;
    public GameObject cue;
    public GameObject handle;
    public GameObject menu_obj;
    public GameObject lazer;
    public GameObject main;
    public GameObject option;
    public Transform camdir;

    public Transform holdPosition;              //큐 고정 위치
    private Rigidbody mPlayer;                  //플레이어

    private GameObject collidingObject;
    private GameObject objectInHand;
    private Vector3 vHoldPos;

    private Mode mmode;
    private CueGrap isGrap;
    private bool cuegrap;
    private bool isJump;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = transform.parent.parent.GetComponent<Rigidbody>();
        mmode = FindObjectOfType<Mode>();
        isGrap = FindObjectOfType<CueGrap>();
        cuegrap = false;
        isJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        PadAction();      //회전 수정 필요 - 그랩문제?
        MenuAction();

        switch (mmode.mode)
        {
            case 0:     // 기본 상태
                CueAction();        //큐 꺼내기
                //GrapAction();       //물건 집기       //수정필요
                break;
            case 1:     // 큐 든 상태
            case 2:     // 메뉴 상태
                CueAction();        //큐 넣기
                Follow();           //큐 위치 지정
                break;
            case 3:     // 물건 든 상태
                //GrapAction();       //물건 놓기/던지기
                break;
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
        Debug.Log("jump");
        mPlayer.AddForce(0, 350, 0);
        yield return new WaitForSeconds(0.8f);
        isJump = false;
    }
    //큐 들기
    private void CueAction()
    {
        if (cue.activeSelf)     //다른 상태에서 돌아올때 큐를 든 상태였으면 큐를 든 상태로
        {
            mmode.mode = 1;
        }
        if (backword.GetStateDown(handType))
        {
            if (!cue.activeSelf)
            {
                Debug.Log("cue on");
                cue.SetActive(true);
                mmode.mode = 1;
            }
            else
            {
                Debug.Log("cue off");
                cue.SetActive(false);
                mmode.mode = 0;
            }
        }
    }

    private void Follow()
    {
        handle.transform.position = transform.position;
        if (isGrap.IsGrap)
        {
            /*
            if (!cuegrap)
            {
                vHoldPos = holdPosition.position;
                cuegrap = true;
            }
            handle.transform.LookAt(vHoldPos);
            */
            handle.transform.LookAt(holdPosition.position);
        }
        else
        {
            handle.transform.rotation = transform.rotation;
            cuegrap = false;
        }
    }

    //물건 들기
    private void GrapAction()
    {
        if (grapAction.GetStateDown(handType))
        {
            if (collidingObject)
            {
                Grap();
            }
        }
        else if (grapAction.GetStateUp(handType))
        {
            if (objectInHand)
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
        if (col.gameObject.layer  != 12)
        {
            return;
        }
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
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
        if (!collidingObject)
        {
            return;
        }
        if (collidingObject.layer.CompareTo("Object") != 0)
        {
            return;
        }
        Debug.Log("Grap");
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
            Debug.Log("Release");
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
                lazer.SetActive(false);
                mmode.mode = 0;
                menu_obj.SetActive(false);
            }
            else    // 메뉴 추가하면 찾아서 초기화 해주어야함!
            {
                menu_obj.SetActive(true);
                main.SetActive(true);
                option.SetActive(false);
                mmode.mode = 2;
                lazer.SetActive(true);
            }
        }
    }

}
