using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARImageTracker : MonoBehaviour
{
    private ARTrackedImageManager m_trackedImageManager;

    [SerializeField] private GameObject _obj;

    private ARTrackedImage _trackedImage;

    private void Awake()
    {
        m_trackedImageManager = GetComponent<ARTrackedImageManager>();        
    }

    private void OnEnable()
    {
        m_trackedImageManager.trackedImagesChanged += OnChanged;
    }

    private void OnDisable()
    {
        m_trackedImageManager.trackedImagesChanged -= OnChanged;
    }

    void Update()
    {
        if(_trackedImage != null)
        {
            // ������ �� �� ���
            if (_trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
            {
                _obj.gameObject.SetActive(false);
            }
            // ������ �� ���
            else if(_trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                _obj.gameObject.SetActive(true);
            }
            else
            {

            }
        }
    }

    private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            // �̹����� ó�� �ν����� �� �߰���
            _trackedImage = newImage;
        }

        foreach(var newImage in eventArgs.updated)
        {
            if(_obj.activeSelf)
            {
                // �����Ǵ� ������Ʈ�� ��ǥ�� ��� ������ ������Ʈ�� ��ǥ�� �����Ѵ�
                _obj.transform.position = newImage.transform.position;
                _obj.transform.rotation = newImage.transform.rotation;
            }
        }
    }
}
