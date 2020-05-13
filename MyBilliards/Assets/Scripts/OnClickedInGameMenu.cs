using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OnClickedInGameMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject option;
    public GameObject mTpMode;
    public GameObject tpcheck;
    public GameObject replay;
    public GameObject assist;
    public Text txt;

    private void Start()
    {
        if (PlayerPrefs.GetInt("tpmode", 0) == 0)
        {
            mTpMode.SetActive(false);
            tpcheck.SetActive(false);
        }
        else
        {
            mTpMode.SetActive(true);
            tpcheck.SetActive(true);
        }
        if (PlayerPrefs.GetInt("assist", 0) == 0)
        {
            assist.SetActive(false);
        }
        else
        {
            assist.SetActive(true);
        }
    }

    public void Replay_btn_clicked()
    {
        //Debug.Log("option");
        main.SetActive(false);
        replay.SetActive(true);
        StartCoroutine(GetText());
    }

    public void getReplay_btn_clicked()
    {
        //Debug.Log("replay");
        main.SetActive(false);
        replay.SetActive(true);
    }

    public void Option_btn_clicked()
    {
        //Debug.Log("option");
        main.SetActive(false);
        option.SetActive(true);
    }

    public void Back_btn_clicked()
    {
        //Debug.Log("bakc");
        main.SetActive(true);
        option.SetActive(false);
        replay.SetActive(false);
    }

    public void Teleport_btn_clicked()
    {
        //Debug.Log("Teleport button");
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

    public void Assist_btn_clicked()
    {
        //Debug.Log("Assist button");
        if (assist.activeSelf)
        {
            assist.SetActive(false);
            PlayerPrefs.SetInt("assist", 0);
        }
        else
        {
            assist.SetActive(true);
            PlayerPrefs.SetInt("assist", 1);
        }
    }

    public void MainMenu_btn_clicked()
    {
        //Debug.Log("MainMenu click");
        SceneManager.LoadScene(0);
    }

    public void Exit_btn_clicked()
    {
        //Debug.Log("exit click");
        PlayerPrefs.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://gtw049nu5f.execute-api.ap-northeast-2.amazonaws.com/default/Helloworld");
        www.SetRequestHeader("x-api-key", "ny8qSXuiWs2liHu1SbCDA3VNCIDnXH5alo8yChU8");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
            txt.text = www.downloadHandler.text;

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
