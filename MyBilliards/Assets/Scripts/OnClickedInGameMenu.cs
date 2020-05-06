using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickedInGameMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject option;
    public GameObject mTpMode;
    public GameObject tpcheck;

    private void Start()
    {
        if (PlayerPrefs.GetInt("tpmode", 0) == 0)
        {
            tpcheck.SetActive(false);
        }
        else
        {
            tpcheck.SetActive(true);
        }
    }

    public void Option_btn_clicked()
    {
        Debug.Log("option");
        main.SetActive(false);
        option.SetActive(true);
    }

    public void Back_btn_clicked()
    {
        Debug.Log("bakc");
        main.SetActive(true);
        option.SetActive(false);
    }

    public void Teleport_btn_clicked()
    {
        Debug.Log("Teleport button");
        if (mTpMode.activeSelf)
        {
            mTpMode.SetActive(false);
            tpcheck.SetActive(false);
            PlayerPrefs.SetInt("tpmode", 0);
        }
        else
        {
            mTpMode.SetActive(true);
            tpcheck.SetActive(true);
            PlayerPrefs.SetInt("tpmode", 1);
        }

    }

    public void MainMenu_btn_clicked()
    {
        Debug.Log("MainMenu click");
        SceneManager.LoadScene(0);
    }

    public void Exit_btn_clicked()
    {
        Debug.Log("exit click");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
