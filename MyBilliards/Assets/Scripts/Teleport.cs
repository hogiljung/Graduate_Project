using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public bool mIsActive {set; get;}

    public int mCount;
    public float mCurveValue;
    public float mGravity;
    public Vector3 mVelocity;
    public Vector3 mGroundPos;
    public List<Transform> mRenderList = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        CreateRender();
    }

    void CreateRender()
    {
        for(int i = 0; i< mCount; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.layer = LayerMask.NameToLayer("Ignore Raycast");
            obj.transform.parent = transform;
            obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            obj.GetComponent<MeshRenderer>().material.color = Color.blue;
            Destroy(obj.GetComponent<BoxCollider>());

            mRenderList.Add(obj.transform);
            mRenderList[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsActive == true)
            ShowPath();
        else
            HidePath();
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

    void CheckGround()
    {
        int closeIdx = 0;
        float dist = 100;
        RaycastHit hit;
        mGroundPos = Vector3.zero;

        for(int i = 0; i < mCount; i++)
        {
            if (mRenderList[i].gameObject.activeSelf == false)
                continue;
            if(Physics.Raycast(mRenderList[i].position,Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") == false)
                    continue;
                float curDist = Vector3.Distance(mRenderList[i].position, hit.point);

                if (dist < curDist)
                    continue;
                closeIdx = i;
                mGroundPos = hit.point;
            }
        }

        for (int i = closeIdx; i < mCount; i++)
            mRenderList[i].gameObject.SetActive(false);
    }
}
