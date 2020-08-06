using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ControllerCharacter : MonoBehaviour
{
    #region Variables

    private CharacterController characterController;
    private NavMeshAgent agent;
    private Camera camera;

    private bool isGrounded = false;
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

    private Vector3 calcVelocity;

    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false; // 네비메쉬 에이전트 내부의 이동기능 끼능
        agent.updateRotation = true;

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 왼쪽 클릭한 지점으로 캐릭터가 이동한다.(NavMeshAgent를 사용해서)

        //마우스 왼쪽 클릭 입력 받기
        if (Input.GetMouseButtonDown(0))
        {
            //씬에서 월드로 향하는 레이캐스트
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //hit 체크
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
            {
                Debug.Log("We hited! " + hit.collider.name + " " + hit.point);

                //캐릭터를 hit된 곳으로 이동
                agent.SetDestination(hit.point);
            }


            //만약 agent의 스톱거리보다 목적지 거리가 더 크다면 -> 아직 더 가야한다
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                characterController.Move(agent.velocity * Time.deltaTime);
            }
            else
            {
                //그게 아니라면 스톱
                characterController.Move(Vector3.zero);
            }

        }
    }


    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }

}
