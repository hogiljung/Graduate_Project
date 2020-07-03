using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class LazerPointer : MonoBehaviour
{
    public SteamVR_Action_Boolean trigger;
    public SteamVR_Input_Sources handType;

    public GameObject particle;         //레이저 파티클

    private LineRenderer layser;        // 레이저
    private RaycastHit hit;             // 충돌된 객체
    private RaycastHit lastHit;
    private GameObject currentObject;   // 가장 최근에 충돌한 객체를 저장하기 위한 객체
    private Vector3 prepose;
    
    private float raycastDistance = 3f; // 레이저 포인터 감지 거리

    // Start is called before the first frame update
    void Start()
    {
        // 스크립트가 포함된 객체에 라인 렌더러라는 컴포넌트를 넣고있다.
        layser = this.gameObject.AddComponent<LineRenderer>();

        // 라인이 가지개될 색상 표현
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0, 195, 255, 0.5f);
        layser.material = material;
        // 레이저의 꼭지점은 2개가 필요 더 많이 넣으면 곡선도 표현 할 수 있다.
        layser.positionCount = 2;
        // 레이저 굵기 표현
        layser.startWidth = 0.002f;
        layser.endWidth = 0.002f;
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
            //Debug.Log("collide");
            layser.SetPosition(1, hit.point);
            particle.transform.position = hit.point;
            particle.SetActive(true);
            // 충돌 객체의 태그가 Button인 경우
            if (hit.collider.gameObject.tag.Equals("Button"))
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
                    if (currentObject)
                    {
                        if (currentObject != hit.collider)
                        {
                            currentObject.GetComponent<Button>().OnPointerExit(null);
                        }
                    }
                    currentObject = hit.collider.gameObject;
                }
            }
            
            else if (hit.collider.gameObject.tag.Equals("Slider"))
            {
                /*
                if (trigger.GetStateDown(handType))
                {
                    Debug.Log("drag point : " + hit.point);
                }
                */
            }
            
            // 캔버스일때
            // 최근 감지된 오브젝트가 Button인 경우
            // 버튼은 현재 눌려있는 상태이므로 풀어준다.
            else if (hit.collider.gameObject.tag.Equals("Canvas"))
            {
                if (currentObject)
                {
                    if (currentObject.tag.Equals("Button"))
                    {
                        currentObject.GetComponent<Button>().OnPointerExit(null);
                    }
                }
            }
            else
            {
                layser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
                particle.SetActive(false);
            }
        }

        else
        {
            // 레이저에 감지된 것이 없기 때문에 레이저 초기 설정 길이만큼 길게 만든다.
            layser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
            particle.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        // 버튼을 누를 경우        
        if (trigger.GetStateDown(handType))
        {
            layser.material.color = new Color(255, 255, 255, 0.5f);
        }

        // 버튼을 뗄 경우          
        else if (trigger.GetStateUp(handType))
        {
            layser.material.color = new Color(0, 195, 255, 0.5f);
        }
    }
}
