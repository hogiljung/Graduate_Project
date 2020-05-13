using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public class Info
    {
        public string ID;
        public Vector3 ball1pos { get; set; }
        public Vector3 ball1rot { get; set; }
        public Vector3 ball2pos { get; set; }
        public Vector3 ball2rot { get; set; }
        public Vector3 ball3pos { get; set; }
        public Vector3 ball3rot { get; set; }
        public string time { get; set; }
    };

    public ReplayforDynamoDB DB;
    public Info data;
    private int count;
    //public float touchTime { get; set; }

    private void Start()
    {
        count = 0;
    }
    public void SetData(Info info) {
        //data = info;
        DB.Saveswinginfo(info);
    }
    

}
