using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    private void Start()
    {
        
    }
    private void Update()
    {
        transform.position.Set(player.transform.localPosition.x, transform.localPosition.y, player.transform.localPosition.z);
    }
}
