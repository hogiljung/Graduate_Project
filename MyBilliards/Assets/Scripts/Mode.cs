using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode : MonoBehaviour
{
    public int mode { set; get; }
    public GameObject mTpMode;

    private void Start()
    {
        mode = 0;
        if (PlayerPrefs.GetInt("tpmode", 0) == 0)
        {
            mTpMode.SetActive(false);
        }
        else
        {
            mTpMode.SetActive(true);
        }
    }
}
