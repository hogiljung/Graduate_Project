using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollide : MonoBehaviour
{
    private string colliderTag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SoundManage.instance.PlaySoundShot("shotStrong");
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliderTag = collision.collider.tag;
        if (colliderTag.Equals("ball"))
        {
            SoundManage.instance.PlaySoundShot("ballCollide");
        }
        else if (colliderTag.Equals("wall"))
        {
            SoundManage.instance.PlaySoundShot("wallCollide");
        }
    }
}
