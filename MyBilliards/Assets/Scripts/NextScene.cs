using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickMenu(int num)
    {
        int nextScene;
        //현재 씬 정보를 가지고 온다
        Scene scene=SceneManager.GetActiveScene();

        //현재 씬의 빌드 순서를 가지고 온다.
        int curScene = num;

        //현재 씬 바로 다음씬을 가져오기 위해 +1을 해준다
        
        // 0:Main 1:GameScene
        if (num == 0) { 
            nextScene = curScene + 1;
            //다음씬을 불러온다.
            SceneManager.LoadScene(nextScene);
        }
        else if (num == 1)
        {
            //리플레이 Scene
        }
        else if (num == 2)
        {
            Application.Quit();
            Debug.Log("게임종료 버튼클릭");
        }

     
      
    }

}
