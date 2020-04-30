using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller_Right : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean teleprotAction;
    public SteamVR_Action_Boolean grabAction;
    public GameObject Camera1;
    public GameObject Camera2;

    private bool cams;

    // Start is called before the first frame update
    void Start()
    {
        cams = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetTeleportDown())
        {
            Debug.Log("Teleport" + handType);
        }
        if (GetGrab())
        {
            /*
            if (cams)
            {
                Camera1.SetActive(false);
                Camera2.SetActive(true);
                cams = false;
            }
            else
            {
                Camera1.SetActive(true);
                Camera2.SetActive(false);
                cams = true;
            }
            */
            Debug.Log("Grap" + handType);
        }
    }

    public bool GetTeleportDown()
    {
        return teleprotAction.GetStateDown(handType);
    }

    public bool GetGrab()
    {
        return grabAction.GetState(handType);
    }

}
