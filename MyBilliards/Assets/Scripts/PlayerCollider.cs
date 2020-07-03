using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(setpos());
    }

    IEnumerator setpos()
    {
        while (true)
        {
            transform.localPosition.Set(player.transform.localPosition.x, this.transform.localPosition.y, player.transform.localPosition.z);
            yield return new WaitForSeconds(0.05f);
        }
    }

}
