using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownCamera))]
//타켓 카메라에 대한 커스터마이징
public class TopDownCamera_SceneEditor : Editor
{
    #region Variables
    private TopDownCamera targetCamera;
    #endregion Variables

    public override void OnInspectorGUI()
    {
        
        targetCamera = (TopDownCamera)target;
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        if(!targetCamera || !targetCamera.target)
        {
            return;
        }

        Transform cameraTarget = targetCamera.target; // 캐릭터
        Vector3 targetPosition = cameraTarget.position;
        targetPosition.y += targetCamera.lookAtHeight; //캐릭터 높이 만큼 더해줌

        // 거리크기의 반지름을 가진 원을 그림
        Handles.color = new Color(1f, 0f, 0f, 0.15f);
        Handles.DrawSolidDisc(targetPosition, Vector3.up, targetCamera.distance);

        Handles.color = new Color(0f, 1f, 0f, 0.75f);
        Handles.DrawWireDisc(targetPosition, Vector3.up, targetCamera.distance);

        // 슬라이더 추가
        Handles.color = new Color(1f, 0f, 0f, 0.5f);
        targetCamera.distance = Handles.ScaleSlider(targetCamera.distance, 
            targetPosition, -cameraTarget.forward, 
            Quaternion.identity, targetCamera.distance, 0.1f);
        targetCamera.distance = Mathf.Clamp(targetCamera.distance, 2f, float.MaxValue);//최대, 최소값 설정

        Handles.color = new Color(0f, 0f, 1f, 0.5f);
        targetCamera.height = Handles.ScaleSlider(targetCamera.height,
            targetPosition, Vector3.up,
            Quaternion.identity, targetCamera.height, 0.1f);
        targetCamera.height = Mathf.Clamp(targetCamera.height, 2f, float.MaxValue);//최대, 최소값 설정

        //레이블 달기
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;

        labelStyle.alignment = TextAnchor.UpperCenter;
        Handles.Label(targetPosition + (-cameraTarget.forward * targetCamera.distance), "Distance", labelStyle);

        labelStyle.alignment = TextAnchor.MiddleRight;
        Handles.Label(targetPosition + (Vector3.up * targetCamera.height), "Height", labelStyle);

        targetCamera.HandleCamera(); // 슬라이더로 변환된 값이 바로 적용되도록 호출
    }

}
