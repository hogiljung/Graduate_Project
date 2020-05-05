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
    public GameObject menu_obj;
    public GameObject lazer;

    private GameObject collidingObject;
    private GameObject objectInHand;
    private Mode mmode;

    // Start is called before the first frame update
    void Start()
    {
        mmode = FindObjectOfType<Mode>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mmode.mode)
        {
            case 0:     // 기본 상태
                CueAction();
                GrapAction();
                MenuAction();
                break;
            case 1:     // 큐 든 상태
                CueAction();

                break;
            case 2:     // 메뉴 상태
                MenuAction();
                break;
            case 3:     // 물건 든 상태
                GrapAction();
                break;
        }
        
    }

    //큐 들기
    private void CueAction()
    {
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
