using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;

public class ARFaceObject : MonoBehaviour
{
    // 배치할 오브젝트의 Prefab을 가져와줌
    [SerializeField] private GameObject _nosePrefab;
    [SerializeField] private GameObject _leftheadPrefab;
    [SerializeField] private GameObject _rightheadPrefab;

    // Type을 입력할 때 빨간줄이 뜨면 alt+enter를 누르면 관련된 패키지 이름이 뜹니다.

    ARFaceManager faceManager; // 얼굴과 관련한 부분을 컨트롤할 것이기 때문에 ARFaceManager를 만듬
    ARSessionOrigin sessionOrigin; // 실제 얼굴 좌표와 매칭하기 위한 SessionOrigin도 만듬

    private NativeArray<ARCoreFaceRegionData> faceRegions; // face의 region 값(코, 양 옆 머리 부분밖에 없긴 함)

    private GameObject nose;
    private GameObject lhead;
    private GameObject rhead;

    bool temp = false;
    private void Awake()
    {
        // 컴포넌트 할당
        faceManager = GetComponent<ARFaceManager>();
        sessionOrigin = GetComponent<ARSessionOrigin>();
    }

    private void Update()
    {
        // subsystem이란?
        // ARFoundation 안에 ARCore, ARKit이 있는데, 특정 SDK의 API만 사용할 수 있도록 만든 것.
        // ARCore subsystem일때와 ARKit subsystem일때 사용할 수 있는 함수가 다름.

        ARCoreFaceSubsystem subsystem = (ARCoreFaceSubsystem)faceManager.subsystem;

        foreach(ARFace face in faceManager.trackables) // ARFaceManager에서 현재 추적중인 모든 얼굴을 순회함
        {
            // 현재 활성화되어있는 모든 얼굴들의 좌표를 가져옴
            subsystem.GetRegionPoses(face.trackableId, Unity.Collections.Allocator.Persistent, ref faceRegions);
            // ARCoreFaceRegionData : 얼굴의 각 부위에 대한 데이터들을 저장하는 구조체이다.
            foreach(ARCoreFaceRegionData faceregion in faceRegions) // 순회 중인 얼굴 1개의 좌표를 순서대로 가져옴
            {
                switch (faceregion.region)
                {
                    case ARCoreFaceRegion.NoseTip:
                        if (nose == null)
                            nose = Instantiate(_nosePrefab, sessionOrigin.trackablesParent);
                        nose.transform.localPosition = faceregion.pose.position;
                        nose.transform.localRotation = faceregion.pose.rotation;
                        break;
                    case ARCoreFaceRegion.ForeheadLeft:
                        if (lhead == null)
                            lhead = Instantiate(_leftheadPrefab, sessionOrigin.trackablesParent);
                        lhead.transform.localPosition = faceregion.pose.position;
                        lhead.transform.localRotation = faceregion.pose.rotation;
                        break;
                    case ARCoreFaceRegion.ForeheadRight:
                        if (rhead == null)
                            rhead = Instantiate(_rightheadPrefab, sessionOrigin.trackablesParent);
                        rhead.transform.localPosition = faceregion.pose.position;
                        rhead.transform.localRotation = faceregion.pose.rotation;
                        break;
                }
            }
        }
    }
}
