using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideUIPos : MonoBehaviour
{
    public Transform character;
    
    void Update()
    {
        if(gameObject.activeSelf)
            transform.LookAt(character);
    }
}
