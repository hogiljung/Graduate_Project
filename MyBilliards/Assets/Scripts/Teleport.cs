using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public bool mIsActive {set; get;}       //텔레포트 조준중인지

    public List<Transform> mRenderList = new List<Transform>();     //렌더 정보 담는 배열
    public GameObject TeleportArea;     //텔레포트 영역표시
    public GameObject denyArea;

    public Vector3 mGroundPos;      //땅에 닿은 지점 좌표
    private Vector3 mVelocity;      //궤적 그리는 변수

    public int mCount;              //궤적 길이(점선 갯수)
    public float mCurveValue;       //점선 간격
    public float mGravity;          //기우는 정도
    private bool IsActive;

    void Start()
    {
        IsActive = false;
        CreateRender();     //궤적 표시용 렌더 생성
    }

    void CreateRender()
    {
        for(int i = 0; i< mCount; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.layer = LayerMask.NameToLayer("Ignore Raycast");
            obj.transform.parent = transform;
            obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.05f);
            obj.GetComponent<MeshRenderer>().material.color = Color.cyan;
            Destroy(obj.GetComponent<BoxCollider>());

            mRenderList.Add(obj.transform);
            mRenderList[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsActive)      //사용하는 동안 궤적 보여주기
        {
            ShowPath();
            IsActive = true;
        }
        else                //사용 안함으로 전환될때 숨기기
        {
            if (IsActive)
            {
                HidePath();
                TeleportArea.SetActive(false);
                denyArea.SetActive(false);
                IsActive = false;
            }
        }
    }

    void HidePath()
    {
        for(int i = 0; i < mCount; i++)
        {
            if (mRenderList[i].gameObject.activeSelf == true)
                mRenderList[i].gameObject.SetActive(false);
        }
    }

    void ShowPath()
    {
        if (mRenderList.Count == 0)
            CreateRender();

        Vector3 pos = transform.position;
        Vector3 g = new Vector3(0, mGravity, 0);
        mVelocity = transform.forward * mCurveValue;

        for(int i = 0; i < mCount; i++)
        {
            float t = i * 0.001f;

            pos = pos + (mVelocity * t) + (0.5f * g * t * t);
            mVelocity += g;
            mRenderList[i].position = pos;
            mRenderList[i].gameObject.SetActive(true);
        }
        CheckGround();
    }

    //땅에 닿은지 체크, 땅이면 그 좌표 저장
    void CheckGround()
    {
        int closeIdx = 0;
        float dist = 100;
        RaycastHit hit;
        mGroundPos = Vector3.zero;

        for(int i = 0; i < mCount; i++)
        {
            if (mRenderList[i].gameObject.activeSelf) {
                if (Physics.Raycast(mRenderList[i].position, Vector3.down, out hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.layer == 10)
                    {
                        float curDist = Vector3.Distance(mRenderList[i].position, hit.point);
                        if (dist > curDist)
                        {
                            closeIdx = i;
                            mGroundPos = hit.point;
                            TeleportArea.transform.position = mGroundPos;
                            TeleportArea.transform.LookAt(new Vector3(transform.position.x, TeleportArea.transform.position.y, transform.position.z));
                            if (!TeleportArea.activeSelf)
                                TeleportArea.SetActive(true);
                            if (denyArea.activeSelf)
                                denyArea.SetActive(false);

                        }
                    }
                    else
                    {
                        float curDist = Vector3.Distance(mRenderList[i].position, hit.point);
                        if (dist > curDist)
                        {
                            closeIdx = i;
                            denyArea.transform.position = hit.point;
                            denyArea.transform.LookAt(new Vector3(transform.position.x, TeleportArea.transform.position.y, transform.position.z));
                            if (TeleportArea.activeSelf)
                                TeleportArea.SetActive(false);
                            if (!denyArea.activeSelf)
                                denyArea.SetActive(true);
                        }
                    }
                }
            }
        }

        for (int i = closeIdx; i < mCount; i++)
            mRenderList[i].gameObject.SetActive(false);
    }
}
