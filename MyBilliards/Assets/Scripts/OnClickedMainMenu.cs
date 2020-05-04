using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickedMainMenu : MonoBehaviour
{

    public void start_btn_clicked()
    {
        Debug.Log("start click");
        SceneManager.LoadScene(1);
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
