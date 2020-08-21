using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider1 : MonoBehaviour
{
    public Transform cam;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position.Set(cam.localPosition.x, 0, cam.localPosition.z);
    }

}
