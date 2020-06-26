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
    public GameObject guideUI1;
    public GameObject guideUI2;
    public GameObject whiteCheck;
    public GameObject yellowCheck;
    public Text GuideText1;
    public Text GuideText2;
    public Text txt;

    public Transform whiteBall;
    public Transform yellowBall;
    public Transform redBall;

    private void Start()
    {
        guideUI1.SetActive(false);
        guideUI2.SetActive(false);
        whiteCheck.SetActive(false);
        yellowCheck.SetActive(false);
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
        //StartCoroutine(GetText());
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

    public void Guide_btn_clicked()
    {
        if (guideUI1.activeSelf)
        {
            guideUI1.transform.position.Set(0,-10f,0);
            whiteCheck.SetActive(false);
            guideUI1.SetActive(false);
            yellowCheck.SetActive(true);
            guideUI2.SetActive(true);
            string[] ballPoints = new string[3];
            ballPoints[0] = (yellowBall.position.x + 1.387) * 64 / 2.774 + "," + (yellowBall.position.z + 0.676) * 32 / 1.352;
            ballPoints[1] = (whiteBall.position.x + 1.387) * 64 / 2.774 + "," + (whiteBall.position.z + 0.676) * 32 / 1.352;
            ballPoints[2] = (redBall.position.x + 1.387) * 64 / 2.774 + "," + (redBall.position.z + 0.676) * 32 / 1.352;
            guideUI2.transform.position = yellowBall.transform.position + new Vector3(0,0.15f,0);
            SetGuideText(GuideText2);
        }
        else if (guideUI2.activeSelf)
        {
            guideUI2.transform.position.Set(0, -10f, 0);
            yellowCheck.SetActive(false);
            guideUI2.SetActive(false);
        }
        else
        {
            whiteCheck.SetActive(true);
            guideUI1.SetActive(true);
            string[] ballPoints = new string[3];
            ballPoints[0] = (whiteBall.position.x + 1.387) * 64 / 2.774 + "," + (whiteBall.position.z + 0.676) * 32 / 1.352;
            ballPoints[1] = (yellowBall.position.x + 1.387) * 64 / 2.774 + "," + (yellowBall.position.z + 0.676) * 32 / 1.352;
            ballPoints[2] = (redBall.position.x + 1.387) * 64 / 2.774 + "," + (redBall.position.z + 0.676) * 32 / 1.352;
            guideUI1.transform.position = whiteBall.transform.position + new Vector3(0, 0.15f, 0);
            SetGuideText(GuideText1);
        }
    }

    private void SetGuideText(Text txt)
    {
        Debug.Log("가이드 업데이트");
        txt.text = "타법 계산중...";
        
        // txt.text = 가이드 결과 입력
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
