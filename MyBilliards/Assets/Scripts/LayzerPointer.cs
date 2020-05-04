using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class LayzerPointer : MonoBehaviour
{
    public SteamVR_Action_Boolean trigger;
    public SteamVR_Input_Sources handType;
    public LineRenderer layser;        // 레이저
    private RaycastHit hit; // 충돌된 객체
    private RaycastHit lastHit;
    private GameObject currentObject;   // 가장 최근에 충돌한 객체를 저장하기 위한 객체
    public float raycastDistance = 10f; // 레이저 포인터 감지 거리

    // Start is called before the first frame update
    void Start()
    {
        // 스크립트가 포함된 객체에 라인 렌더러라는 컴포넌트를 넣고있다.

        // 라인이 가지개될 색상 표현
        // 레이저의 꼭지점은 2개가 필요 더 많이 넣으면 곡선도 표현 할 수 있다.
        // 레이저 굵기 표현
    }

    // Update is called once per frame
    void Update()
    {
        layser.SetPosition(0, transform.position); // 첫번째 시작점 위치
                                                   // 업데이트에 넣어 줌으로써, 플레이어가 이동하면 이동을 따라가게 된다.
                                                   //  선 만들기(충돌 감지를 위한)
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);

        // 충돌 감지 시
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            Debug.Log("collide");
            layser.SetPosition(1, hit.point);

            // 충돌 객체의 태그가 Button인 경우
            if (hit.collider.gameObject.CompareTag("Button"))
            {
                // 트리거 버튼 누르면
                if (trigger.GetStateDown(handType))
                {
                    // 버튼에 등록된 onClick 메소드를 실행
                    hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                }

                else
                {
                    hit.collider.gameObject.GetComponent<Button>().OnPointerEnter(null);
                    currentObject = hit.collider.gameObject;
                }
            }
            else
            {
                currentObject = hit.collider.gameObject;
            }
        }

        else
        {
            // 레이저에 감지된 것이 없기 때문에 레이저 초기 설정 길이만큼 길게 만든다.
            layser.SetPosition(1, transform.position + (transform.forward * raycastDistance));

            // 최근 감지된 오브젝트가 Button인 경우
            // 버튼은 현재 눌려있는 상태이므로 이것을 풀어준다.
            if (currentObject != null)
            {
                currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }

        }

    }
    private void LateUpdate()
    {
        // 버튼을 누를 경우        
        if (trigger.GetStateDown(handType))
        {
            Debug.Log("click1");
            layser.material.color = new Color(255, 255, 255, 0.5f);
        }

        // 버튼을 뗄 경우          
        else if (trigger.GetStateUp(handType))
        {
            layser.material.color = new Color(0, 195, 255, 0.5f);
        }
    }
}
