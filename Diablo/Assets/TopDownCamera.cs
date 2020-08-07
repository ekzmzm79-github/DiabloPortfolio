﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    #region Variables
    public float height = 5f;
    public float distance = 10f;
    public float angle = 45f;
    public float lookAtHeight = 2f;
    public float smoothSpeed = 0.5f;

    private Vector3 refVelocity;

    public Transform target;

    #endregion Variables

    private void LateUpdate()
    {
        HandleCamera();
    }

    public void HandleCamera()
    {
        if (!target)
            return;

        // worldPosition 만들기 (타겟보다 약간 뒤쪽에 있어야 하므로 -distance
        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
        //Debug.DrawLine(target.position, worldPosition, Color.red);

        //회전 벡터
        Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
        //Debug.DrawLine(target.position, rotatedVector, Color.green);

        Vector3 finalTargetPosition = target.position;
        finalTargetPosition.y += lookAtHeight; // 현재 타겟(캐릭터의 중심점은 발바닥이므로 높이값을 더해줌)

        Vector3 finalPosition = finalTargetPosition + rotatedVector;
        //Debug.DrawLine(target.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt(target.position);
    }

    /// <summary>
    /// 디버그용 위치확인 라인 및 원구 그리기
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        if(target)
        {
            Vector3 lookAtPosition = target.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f); //바라보는 방향에 원구형태 그리기
        }

        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
