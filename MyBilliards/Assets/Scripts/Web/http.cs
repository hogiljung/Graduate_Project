using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class http : MonoBehaviour
{
    [SerializeField]
    public class BallInfo
    {
        public string user_id;
        public string ball_id;
        public int force;
        public string hitdot;
        public string position;
        public string rotation;
        public string time;

    }

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://gtw049nu5f.execute-api.ap-northeast-2.amazonaws.com/default/Helloworld");
        www.SetRequestHeader("x-api-key", "ny8qSXuiWs2liHu1SbCDA3VNCIDnXH5alo8yChU8");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}