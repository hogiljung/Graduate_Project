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
    DynamoDBContext context;
    AmazonDynamoDBClient DBclient;
    CognitoAWSCredentials credentials;
    private void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        this.gameObject.AddComponent<CustomUnityMainThreadDispatcher>();
        Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;
        credentials = new CognitoAWSCredentials("ap-northeast-2:c9be57d7-f34a-458a-b54c-0cbf5de1df78", RegionEndpoint.APNortheast2);
        DBclient = new AmazonDynamoDBClient(credentials, RegionEndpoint.APNortheast2);
        context = new DynamoDBContext(DBclient);

      //  Requestreplayinfo();
    }

    [DynamoDBTable("Ballinfo")]
    public class Swing
    {
        [DynamoDBHashKey] // Hash key.
        public string user_id { get; set; }
        [DynamoDBProperty]
        public string ball_id { get; set; }
        [DynamoDBProperty]
        public string position { get; set; }
        [DynamoDBProperty]
        public string rotation { get; set; }
        [DynamoDBProperty]
        public int force { get; set; }
        [DynamoDBProperty]
        public string hitdot { get; set; }
        [DynamoDBProperty]
        public string time { get; set; }

    }


    public void Saveswinginfo() //볼 정보를 DB에 올리기
    {
        Swing c1 = new Swing
        {
            user_id = "aass",
            ball_id = "Whiteball",
            position = "0.1,0.3,0.1",
            rotation = "1,2,3",
            force = 5,
            hitdot = "0.09,0.3,0.09",
            time = "2020.01.15",
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
        Swing c;
        context.LoadAsync<Swing>("abcd", (AmazonDynamoDBResult<Swing> result) =>
        {
            // id가 abcd인 볼 정보를 DB에서 받아옴
            if (result.Exception != null)
            {
                Debug.LogException(result.Exception);
                return;
            }
            c = result.Result;
            Debug.Log("유저ID: " + c.user_id); //볼 정보 출력
            Debug.Log("볼 ID: " + c.ball_id);
            Debug.Log("좌표: " + c.position);
            Debug.Log("각도: " + c.rotation);
            Debug.Log("힘: " + c.force);
            Debug.Log("타격 점: " + c.hitdot);
            Debug.Log("시각: " + c.time);

        }, null);

    }

    public void RequestTable()
    {
      //  ScanCondition test = new ScanCondition("user_id",)


    //    context.ScanAsync("Ballinfo",);
   
    }





}
