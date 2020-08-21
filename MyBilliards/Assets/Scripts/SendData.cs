using System.Collections;
using UnityEngine;
using SocketIO;
using System.Collections.Generic;
using ScrollUI;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class SendData : MonoBehaviour
{
    private SocketIOComponent socket;
    // Dictionary<string, string> data = new Dictionary<string, string>();
    JSONObject jdata;

    List<ArrayList> frame;


    public GameObject ball1;
    public GameObject ball2;
    public GameObject ball3;
    public GameObject que;

    public Text guidtxt1;
    public Text guidtxt2;

    private Transform ball1trans;
    private Transform ball2trans;
    private Transform ball3trans;
    private Transform quetrans;


    Vector3[,] vec;


    private string tablename;

    private float pos_x;
    private float pos_y;
    private float pos_z;


    private float rot_x;
    private float rot_y;
    private float rot_z;

    int curruntFrame = 0;

    int index = 0;
    private int tablerow = 0;

    private bool test = false;

    public bool startREC { get; set; }
    private bool startReplay;

    public string recommendedStroke { get; set; }
    public string[] ballPoints { get; set; }

    public string[] STROKE = new string[7] { "뒤돌리기", "앞돌리기", "옆돌리기", "빗겨치기", "대회전", "더블레일", "빈쿠션" };


    [SerializeField] GameObject button;
    public RectTransform Canvas;



    // Start is called before the first frame update
    void Start()
    {
        frame = new List<ArrayList>();
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        ball1trans = ball1.transform;
        ball2trans = ball2.transform;
        ball3trans = ball3.transform;
        quetrans = que.transform;


        socket.On("SetReplayBoard", ReplayBoard);
        socket.On("PlayReplay", PlayReplaying);

        socket.On("RecieveStrokeCommend", rcvRecommendedStroke);
        socket.On("ReturnTableRow", ReturnTableRow);

        ballPoints = new string[3];


        // 사용 변수 초기화
        Init();
        startREC = false;
        startReplay = false;

    }


    // Update is called once per frame
    void Update()
    {


        if (startREC)
            SendFrameByUnity();
        if (startReplay)
            ExcuteReplay();

    }

    /// <summary>
    /// 큐, 당구공 3개의 위치값, 회전값을 서버로 전달
    /// 서버에 SendDatabyUnity 이벤트 발생
    /// </summary>
    public void SendFrameByUnity()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data.Add("ball1pos_x", ball1trans.position.x.ToString());
        data.Add("ball1pos_y", ball1trans.position.y.ToString());
        data.Add("ball1pos_z", ball1trans.position.z.ToString());

        data.Add("ball1rot_x", ball1trans.rotation.x.ToString());
        data.Add("ball1rot_y", ball1trans.rotation.y.ToString());
        data.Add("ball1rot_z", ball1trans.rotation.z.ToString());

        data.Add("ball2pos_x", ball2trans.position.x.ToString());
        data.Add("ball2pos_y", ball2trans.position.y.ToString());
        data.Add("ball2pos_z", ball2trans.position.z.ToString());

        data.Add("ball2rot_x", ball2trans.rotation.x.ToString());
        data.Add("ball2rot_y", ball2trans.rotation.y.ToString());
        data.Add("ball2rot_z", ball2trans.rotation.z.ToString());

        data.Add("ball3pos_x", ball3trans.position.x.ToString());
        data.Add("ball3pos_y", ball3trans.position.y.ToString());
        data.Add("ball3pos_z", ball3trans.position.z.ToString());

        data.Add("ball3rot_x", ball3trans.rotation.x.ToString());
        data.Add("ball3rot_y", ball3trans.rotation.y.ToString());
        data.Add("ball3rot_z", ball3trans.rotation.z.ToString());

        data.Add("quepos_x", quetrans.position.x.ToString());
        data.Add("quepos_y", quetrans.position.y.ToString());
        data.Add("quepos_z", quetrans.position.z.ToString());

        data.Add("querot_x", quetrans.rotation.x.ToString());
        data.Add("querot_y", quetrans.rotation.y.ToString());
        data.Add("querot_z", quetrans.rotation.z.ToString());



        jdata = new JSONObject(data);

        socket.Emit("SendDatabyUnity", jdata);



        Debug.Log("데이터 입력" + ball1trans.position);

    }
    /// <summary>
    /// 테이블 생성 서버로 CreateTable 이벤트 발생
    /// </summary>
    public void CreateReplay()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("user_id", "junoung");

        jdata = new JSONObject(data);
        socket.Emit("CreateTable", jdata);
        startREC = true;
        Debug.Log("테이블 생성");

    }

    public void StopREC()
    {
        startREC = false;
    }


    public void Init()
    {
        pos_x = 0f; pos_y = 0f; pos_z = 0f;
        rot_x = 0f; rot_y = 0f; rot_z = 0f;

    }

    /// <summary>
    /// 리플레이 화면 요청
    /// 서버로 RequestReplayByUnity 이벤트 발생
    /// </summary>
    public void RequestReplayByUnity()
    {
        Debug.Log("리플레이 화면 요청");
        socket.Emit("RequestReplayByUnity");

    }
    /// <summary>
    /// RequestReplayByUnity(Unity) -> SetReplayBoard 이벤트 발생(node) -> ReplayBoard (this) 실행
    /// 실행후 InitForReplayBoard 함수 실행
    /// </summary>
    /// <param name="e"> 모든 리플레이 목록 리스트 반환 데이터 </param>
    public void ReplayBoard(SocketIOEvent e)
    {

        InitForReplayBoard(e.data.list.Count, e.data.keys, e.data);

    }
    /// <summary>
    /// 서버로 부터 받은 리플레이 목록으로 Unity 상에 동적 스크롤 뷰의 버튼 생성
    /// </summary>
    /// <param name="len">리플레이 갯수 </param>
    /// <param name="keys">리플레이명 </param>
    /// <param name="values">저장날짜 </param>
    public void InitForReplayBoard(int len, List<string> keys, JSONObject values)
    {
        int yValue = 0;
        List<GameObject> buttonlist = new List<GameObject>();

        int children = GameObject.Find("Content").transform.childCount;
        //버튼 있으면 삭제
        if (children > 0)
        {
            for (int i = 0; i < children; i++)
            {
                Destroy(GameObject.Find("Content").transform.GetChild(i).gameObject);
            }
        }

        //버튼 생성
        for (int i = 0; i < len; i++)
        {

            GameObject newItem = (GameObject)Instantiate(button);
            newItem.transform.SetParent(GameObject.Find("Content").transform);
            newItem.GetComponent<RectTransform>().localPosition = new Vector3(0, yValue, 0);
            newItem.GetComponent<RectTransform>().localRotation = Quaternion.Euler(Vector3.zero);
            newItem.GetComponent<RectTransform>().localScale.Set(1, 1, 1);
            newItem.GetComponent<BoxCollider>().isTrigger = true;
            newItem.GetComponent<BoxCollider>().size = new Vector3(580.0f, 40.0f, 0.000025f);      //1,1,1
            Debug.Log("col size : " + newItem.GetComponent<BoxCollider>().size);
            newItem.GetComponent<Button>().onClick.AddListener(() => ReplayButtonOnClick(newItem.name));
            newItem.name = keys[i];

            Text title = newItem.GetComponentInChildren<Text>();
            title.text = keys[i] + "\t\t\t" + "저장날짜" + values[i].ToString();


            // Button index = Instantiate(newItem, new Vector3(0, yValue, 0), Quaternion.identity);

            // index.transform.SetParent(GameObject.Find("Content").transform);


            yValue -= 200;
        }


    }
    /// <summary>
    /// 리플레이 목록 버튼 클릭 이벤트
    /// </summary>
    /// <param name="name"></param>
    public void ReplayButtonOnClick(string name)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("title", name);

        tablename = name;

        jdata = new JSONObject(data);

        //Debug.Log("ReplayButtonOnClick:"+name);
        socket.Emit("RequestTableRow", jdata);
        vec = new Vector3[tablerow, 8];
        socket.Emit("RequestFrameByUnity", jdata);

    }

    /// <summary>
    /// ReplayButtonOnClick(선택한 리플레이명) -> RequestFrameByUnity(Unity) -> PlayReplay (node) -> this 실행
    /// </summary>
    /// <param name="e">리플레이 프레임 정보</param>
    public void PlayReplaying(SocketIOEvent e)
    {
        // DB 행 갯수 가져오기
        Debug.Log("서버에서 가져옴");

        Debug.Log("index포문: " + index);
        vec[index, 0] = new Vector3(float.Parse(e.data[0].ToString()), float.Parse(e.data[1].ToString()), float.Parse(e.data[2].ToString()));
        vec[index, 1] = new Vector3(float.Parse(e.data[3].ToString()), float.Parse(e.data[4].ToString()), float.Parse(e.data[5].ToString()));
        vec[index, 2] = new Vector3(float.Parse(e.data[6].ToString()), float.Parse(e.data[7].ToString()), float.Parse(e.data[8].ToString()));
        vec[index, 3] = new Vector3(float.Parse(e.data[9].ToString()), float.Parse(e.data[10].ToString()), float.Parse(e.data[11].ToString()));
        vec[index, 4] = new Vector3(float.Parse(e.data[12].ToString()), float.Parse(e.data[13].ToString()), float.Parse(e.data[14].ToString()));
        vec[index, 5] = new Vector3(float.Parse(e.data[15].ToString()), float.Parse(e.data[16].ToString()), float.Parse(e.data[17].ToString()));
        vec[index, 6] = new Vector3(float.Parse(e.data[18].ToString()), float.Parse(e.data[19].ToString()), float.Parse(e.data[20].ToString()));
        vec[index, 7] = new Vector3(float.Parse(e.data[21].ToString()), float.Parse(e.data[22].ToString()), float.Parse(e.data[23].ToString()));
        Debug.Log("vec rec: " + vec[index, 0]);

        index++;
        // Debug.Log("처음"+vec[10, 2]);
        startReplay = true;

    }


    // 입력 데이터 전송 함수
    public void getRecommendedStroke()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        //ballPoints = { "1,2", "2,3", "3,4"};\
        Debug.Log("guide2" + ballPoints[0]);
        Debug.Log(ballPoints[1]);
        Debug.Log(ballPoints[2]);

        data.Add("point1", ballPoints[0]);
        data.Add("point2", ballPoints[1]);
        data.Add("point3", ballPoints[2]);


        // 전송 데이터
        jdata = new JSONObject(data);
        socket.Emit("GetCommendedStroke", jdata);
    }

    // 결과 수신 및 변수 저장 함수
    public void rcvRecommendedStroke(SocketIOEvent e)
    {
        string pos = e.data["result"].ToString();
        Debug.Log("e.data[0], " + e.data[0].ToString());
        Debug.Log("x: " + pos[2] + ", y:" + pos.Substring(4, 4));

        // 예측 결과
        recommendedStroke = STROKE[int.Parse(pos[2].ToString())] + ", " + float.Parse(pos.Substring(4, 4)) * 100 + "%";    //STROKE[int.Parse(e.data[0].ToString())] + ", " + e.data[1] + "%";

        guidtxt1.text = recommendedStroke;
        guidtxt2.text = recommendedStroke;
        Debug.Log("guid1 " + recommendedStroke);
    }

    public void ExcuteReplay()
    {
        Debug.Log("vec ball1 pos: " + vec[curruntFrame, 0]);

        ball1trans.position = vec[curruntFrame, 0];
        ball1trans.rotation = Quaternion.Euler(vec[curruntFrame, 1]);
        ball2trans.position = vec[curruntFrame, 2];
        ball2trans.rotation = Quaternion.Euler(vec[curruntFrame, 3]);
        ball3trans.position = vec[curruntFrame, 4];
        ball3trans.rotation = Quaternion.Euler(vec[curruntFrame, 5]);
        quetrans.position = vec[curruntFrame, 6];
        quetrans.rotation = Quaternion.Euler(vec[curruntFrame, 7]);


        Debug.Log("ball1 pos: " + ball1trans.position);
        Debug.Log("ball1 rot: " + ball1trans.rotation);


        curruntFrame++;

        if (curruntFrame == 100)
        {
            startReplay = false;
            curruntFrame = 0;
        }

    }

    /// <summary>
    /// 서버로 리플레이 목록의 갯수 요청
    /// 반환 함수  ReturnTableRow(SocketIOEvent e)
    /// </summary>
    public void RequestTableRow()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("tablename", tablename);

        jdata = new JSONObject(data);
        socket.Emit("ReturnTableRow", jdata);


    }
    /// <summary>
    /// 서버로 부터 리플레이 목록 갯수 반환
    /// </summary>
    /// <param name="e">리플레이 목록 갯수</param>
    public void ReturnTableRow(SocketIOEvent e)
    {
        // Debug.Log("Returnrow:"+e.data);
        string[] array = e.data.ToString().Split(':');
        //Debug.Log("test" + array[2]);
        //array[2].Remove(array[2].Length - 2, 2);
        tablerow = int.Parse(array[2].Replace("}", ""));
        Debug.Log("tbrow " + tablerow);


    }




}