using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter : MonoBehaviour
{
    #region Variables
    public float speed = 5.0f;
    public float jumpHeight = 2.0f;
    public float dashDistance = 5.0f;

    public float gravity = -9.81f;
    public Vector3 drags;

    private CharacterController characterController;
    private Vector3 inputDirection = Vector3.zero;

    private bool isGrounded = false;
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

    private Vector3 calcVelocity;

    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //현재 캐릭터가 땅인지 아닌지를 검사

        isGrounded = characterController.isGrounded;
        if (isGrounded && calcVelocity.y < 0)
        {
            calcVelocity.y = 0;
        }
        //CheckGroundStatus();

        //사용자의 입력을 이동입력 받기
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * Time.deltaTime * speed);
        if (move != Vector3.zero)
        {
            transform.forward = inputDirection;
        }

        //점프 입력 처리
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            calcVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y);
        }

        //대쉬 입력 처리
        if (Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward,
                dashDistance * new Vector3(
                (Mathf.Log(1f / (Time.deltaTime * drags.x + 1)) / -Time.deltaTime),
                0,
                (Mathf.Log(1f / (Time.deltaTime * drags.z + 1)) / -Time.deltaTime)
                ));

            calcVelocity += dashVelocity;
        }

        //중력값 계산
        calcVelocity.y += gravity * Time.deltaTime;

        calcVelocity.x /= 1 + drags.x * Time.deltaTime;
        calcVelocity.y /= 1 + drags.y * Time.deltaTime;
        calcVelocity.z /= 1 + drags.z * Time.deltaTime;

        characterController.Move(calcVelocity * Time.deltaTime);
    }

    #region Helper Methods

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

#if UNITY_EDITOR
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f),
            transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif

        //캐릭터의 바닥에서(조금 위) 아래 방향으로 레이캐스트 발사
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance, groundLayerMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    #endregion Helper Methods

}
