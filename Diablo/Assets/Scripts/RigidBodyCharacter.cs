using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCharacter : MonoBehaviour
{
    #region Variables
    public float speed = 5.0f;
    public float jumpHeight = 2.0f;
    public float dashDistance = 5.0f;

    private Rigidbody rigidbody;
    private Vector3 inputDirection = Vector3.zero;

    private bool isGrounded = false;
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;
    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //현재 캐릭터가 땅인지 아닌지를 검사
        CheckGroundStatus();

        inputDirection = Vector3.zero;
        //사용자의 입력을 inputDirection에 받기
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.z = Input.GetAxis("Vertical");
        if (inputDirection != Vector3.zero)
        {
            transform.forward = inputDirection;
        }

        //점프 입력 처리
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y);
            rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);
        }

        //대쉬 입력 처리
        if (Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward,
                dashDistance * new Vector3(
                (Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime),
                0,
                (Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime)
                ));

            rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + inputDirection * speed * Time.fixedDeltaTime);
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
