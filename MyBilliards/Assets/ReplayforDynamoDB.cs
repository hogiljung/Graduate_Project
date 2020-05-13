using UnityEngine;
using UnityEngine.SceneManagement;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime.Internal;
using System.Collections.Generic;




public class ReplayforDynamoDB : MonoBehaviour
{
    public SaveData sd;
    DynamoDBContext context;
    AmazonDynamoDBClient DBclient;
    CognitoAWSCredentials credentials;
    private void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        this.gameObject.AddComponent<CustomUnityMainThreadDispatcher>();
        Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;
        credentials = new CognitoAWSCredentials("ap-northeast-2:c9be57d7-f34a-458a-b54c-0cbf5de1df78", RegionEndpoint.APNortheast2);
        DBclient = new AmazonDynamoDBClient(credentials, RegionEndpoint.APNortheast2);
        context = new DynamoDBContext(DBclient);
        
    }

    [DynamoDBTable("Ballinfo")]
    public class Swings
    {
        [DynamoDBHashKey] // Hash key.
        public string user_id { get; set; }
        [DynamoDBProperty]
        public string ball1pos { get; set; }
        [DynamoDBProperty]
        public string ball1rot { get; set; }
        [DynamoDBProperty]
        public string ball2pos { get; set; }
        [DynamoDBProperty]
        public string ball2rot { get; set; }
        [DynamoDBProperty]
        public string ball3pos { get; set; }
        [DynamoDBProperty]
        public string ball3rot { get; set; }
        [DynamoDBProperty]
        public string time { get; set; }

    }


    public void Saveswinginfo(SaveData.Info ts) //볼 정보를 DB에 올리기
    {
        //Debug.Log("ts " + ts.ball1pos);
        Swings c1 = new Swings
        {
            user_id = ts.ID.ToString(),
            ball1pos = ts.ball1pos.ToString(),
            ball1rot = ts.ball1rot.ToString(),
            ball2pos = ts.ball1pos.ToString(),
            ball2rot = ts.ball1rot.ToString(),
            ball3pos = ts.ball1pos.ToString(),
            ball3rot = ts.ball1rot.ToString(),
            time = ts.time
        };
        context.SaveAsync(c1, (result) =>
        {
            //id가 aass인 볼 정보를 DB에 저장
            if (result.Exception == null)
                Debug.Log("Success!");
            else
                Debug.Log(result.Exception);
        });
    }


    public void Requestreplayinfo() //DB에서 볼 정보 받기
    {
        Swings c;
        context.LoadAsync<Swings>("abcd", (AmazonDynamoDBResult<Swings> result) =>
        {
            // id가 abcd인 볼 정보를 DB에서 받아옴
            if (result.Exception != null)
            {
                Debug.LogException(result.Exception);
                return;
            }
            c = result.Result;
            /*
            Debug.Log("유저ID: " + c.user_id); //볼 정보 출력
            Debug.Log("볼 ID: " + c.ball_id);
            Debug.Log("좌표: " + c.position);
            Debug.Log("각도: " + c.rotation);
            Debug.Log("힘: " + c.force);
            Debug.Log("타격 점: " + c.hitdot);
            Debug.Log("시각: " + c.time);
            */

        }, null);

    }
    





}
