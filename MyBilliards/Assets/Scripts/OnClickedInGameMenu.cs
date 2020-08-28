using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using System;

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
    public Text heightText;
    //public Text txt;

    public Transform whiteBall;
    public Transform yellowBall;
    public Transform redBall;
    public Transform camdir;

    private SendData sd;
    private float height;
    
    private void Start()
    {
        guideUI1.SetActive(false);
        guideUI2.SetActive(false);
        whiteCheck.SetActive(false);
        yellowCheck.SetActive(false);

        sd = FindObjectOfType<SendData>();

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

        height = PlayerPrefs.GetFloat("height", 0);
        camdir.localPosition = new Vector3(0, height, 0);
        heightText.text = "높이 보정 : " + height + " cm";

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            Guide_btn_clicked();
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

    public void Up_btn_clicked()
    {
        height += 0.05f;
        height = (float)Math.Round(height * 100) / 100;
        camdir.localPosition = new Vector3(0, height, 0);
        heightText.text = "높이 보정 : " + height + " cm";
        PlayerPrefs.SetFloat("height", height);
    }

    public void Down_btn_clicked()
    {
        height -= 0.05f;
        height = (float)Math.Round(height * 100) / 100;
        camdir.localPosition = new Vector3(0, height, 0);
        heightText.text = "높이 보정 : " + height + " cm";
        PlayerPrefs.SetFloat("height", height);
    }

    //가이드버튼 클릭시 가이드표시 변경
    public void Guide_btn_clicked()
    {
        if (guideUI1.activeSelf)    //흰공 on에서 노란공 on으로 변경
        {
            guideUI1.transform.position.Set(0,-10f,0);
            whiteCheck.SetActive(false);
            guideUI1.SetActive(false);
            yellowCheck.SetActive(true);
            guideUI2.SetActive(true);
            string[] ballPoints = new string[3];
            ballPoints[0] = (int)((yellowBall.localPosition.x + 1.387f) * 64 / 2.774f) + "," + (int)((yellowBall.localPosition.z + 0.676f) * 32 / 1.352f);
            ballPoints[1] = (int)((whiteBall.localPosition.x + 1.387f) * 64 / 2.774f) + "," + (int)((whiteBall.localPosition.z + 0.676f) * 32 / 1.352f);
            ballPoints[2] = (int)((redBall.localPosition.x + 1.387f) * 64 / 2.774f) + "," + (int)((redBall.localPosition.z + 0.676f) * 32 / 1.352f);
            SetGuideText(GuideText2);
            //Debug.Log(ballPoints);
            sd.ballPoints = ballPoints;
            sd.getRecommendedStroke();

            guideUI2.transform.position = yellowBall.position + new Vector3(0,0.15f,0);
        }
        else if (guideUI2.activeSelf)   //노란공 on에서 off로 변경
        {
            guideUI2.transform.position.Set(0, -10f, 0);
            yellowCheck.SetActive(false);
            guideUI2.SetActive(false);
        }
        else                            //off에서 흰공 on으로 변경
        {
            whiteCheck.SetActive(true);
            guideUI1.SetActive(true);
            string[] ballPoints = new string[3];
            ballPoints[0] = (int)((whiteBall.localPosition.x + 1.387f) * 64 / 2.774f) + "," + (int)((whiteBall.localPosition.z + 0.676f) * 32 / 1.352f);
            ballPoints[1] = (int)((yellowBall.localPosition.x + 1.387f) * 64 / 2.774f) + "," + (int)((yellowBall.localPosition.z + 0.676f) * 32 / 1.352f);
            ballPoints[2] = (int)((redBall.localPosition.x + 1.387f) * 64 / 2.774f) + "," + (int)((redBall.localPosition.z + 0.676f) * 32 / 1.352f);
            SetGuideText(GuideText1);

            Debug.Log("guide" + ballPoints[0]);
            Debug.Log(ballPoints[1]);
            Debug.Log(ballPoints[2]);

            sd.ballPoints = ballPoints;
            sd.getRecommendedStroke();
            guideUI1.transform.position = whiteBall.position + new Vector3(0, 0.15f, 0);
        }
    }

    //가이드 텍스트 수정함수
    private void SetGuideText(Text txt)
    {
        //Debug.Log("가이드 업데이트");
        txt.text = "타법 계산중...";
        
        // txt.text = 가이드 결과 입력
    }

    //시작 화면으로
    public void MainMenu_btn_clicked()
    {
        //Debug.Log("MainMenu click");
        SceneManager.LoadScene(0);
    }

    //종료
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

    //리셋
    public void Reset_btn_clicked()
    {
        SceneManager.LoadScene(1);
    }

    //가이드 결과 가져오기
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
            Debug.Log(www.downloadHandler.text);
            //txt.text = www.downloadHandler.text;

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
