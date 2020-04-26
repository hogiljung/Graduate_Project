using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller_Left : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean forword;
    public SteamVR_Action_Boolean backword;
    public SteamVR_Action_Boolean left;
    public SteamVR_Action_Boolean right;
    public SteamVR_Action_Boolean TouchPad;
    public SteamVR_Action_Vector2 TouchPos;
    public SteamVR_TrackedObject mTrackedObj;
    public GameObject player;

    public Teleport mTeleport;
    public Transform mPlayer;

    private float speed;
    private bool move;
    public bool mode;
    // Start is called before the first frame update
    void Start()
    {
        mPlayer = transform.parent;
        mTeleport = FindObjectOfType<Teleport>();
        mTrackedObj = GetComponent<SteamVR_TrackedObject>();
        speed = 0.3f;
        move = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 왼손 터치패드 동작
        if (mode)
            Moving();
        else
            Teleporting();
    }

    private void Moving()
    {
        if (GetForwordDown())
        {
            Debug.Log("forword");
            player.transform.Translate(player.transform.forward * speed);
        }

        if (GetBackwordDown())
        {
            Debug.Log("backword");
            player.transform.Translate(player.transform.forward * -speed);
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
    }

    private void Teleporting()
    {
        if (TouchPad.GetStateDown(handType))
        {
            if (mTeleport)
                mTeleport.mIsActive = true;

        }
        else
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
