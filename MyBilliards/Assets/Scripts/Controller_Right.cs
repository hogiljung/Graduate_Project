using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Controller_Right : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean teleprotAction;
    public SteamVR_Action_Boolean grabAction;

    // Start is called before the first frame update
    void Start()
    {

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
