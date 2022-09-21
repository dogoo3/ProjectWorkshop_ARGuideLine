using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;

public class ARFaceObject : MonoBehaviour
{
    // ��ġ�� ������Ʈ�� Prefab�� ��������
    [SerializeField] private GameObject _nosePrefab;
    [SerializeField] private GameObject _leftheadPrefab;
    [SerializeField] private GameObject _rightheadPrefab;

    // Type�� �Է��� �� �������� �߸� alt+enter�� ������ ���õ� ��Ű�� �̸��� ��ϴ�.

    ARFaceManager faceManager; // �󱼰� ������ �κ��� ��Ʈ���� ���̱� ������ ARFaceManager�� ����
    ARSessionOrigin sessionOrigin; // ���� �� ��ǥ�� ��Ī�ϱ� ���� SessionOrigin�� ����

    private NativeArray<ARCoreFaceRegionData> faceRegions; // face�� region ��(��, �� �� �Ӹ� �κйۿ� ���� ��)

    private GameObject nose;
    private GameObject lhead;
    private GameObject rhead;

    bool temp = false;
    private void Awake()
    {
        // ������Ʈ �Ҵ�
        faceManager = GetComponent<ARFaceManager>();
        sessionOrigin = GetComponent<ARSessionOrigin>();
    }

    private void Update()
    {
        // subsystem�̶�?
        // ARFoundation �ȿ� ARCore, ARKit�� �ִµ�, Ư�� SDK�� API�� ����� �� �ֵ��� ���� ��.
        // ARCore subsystem�϶��� ARKit subsystem�϶� ����� �� �ִ� �Լ��� �ٸ�.

        ARCoreFaceSubsystem subsystem = (ARCoreFaceSubsystem)faceManager.subsystem;

        foreach(ARFace face in faceManager.trackables) // ARFaceManager���� ���� �������� ��� ���� ��ȸ��
        {
            // ���� Ȱ��ȭ�Ǿ��ִ� ��� �󱼵��� ��ǥ�� ������
            subsystem.GetRegionPoses(face.trackableId, Unity.Collections.Allocator.Persistent, ref faceRegions);
            // ARCoreFaceRegionData : ���� �� ������ ���� �����͵��� �����ϴ� ����ü�̴�.
            foreach(ARCoreFaceRegionData faceregion in faceRegions) // ��ȸ ���� �� 1���� ��ǥ�� ������� ������
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
