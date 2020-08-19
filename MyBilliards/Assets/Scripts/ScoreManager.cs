using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int cusion;      //쿠션 충돌 수
    public int score;       //점수
    public int ball;        //0 = 흰 1 = 노랑
    public bool success;    //3쿠션 성공 여부
    public int isEnd;       //정지한 공 카운트

    void Start()
    {
        cusion = 0;
        score = 0;
        ball = -1;
        success = false;
        isEnd = 0;
    }

    //세팅 초기화(점수 제외)
    public void TurnEnd()
    {
        cusion = 0;
        ball = -1;
        success = false;
        isEnd = 0;
    }
}
