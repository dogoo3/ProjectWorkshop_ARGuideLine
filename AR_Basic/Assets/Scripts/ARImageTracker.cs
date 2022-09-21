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
            // 추적이 안 될 경우
            if (_trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
            {
                _obj.gameObject.SetActive(false);
            }
            // 추적이 될 경우
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
            // 이미지를 처음 인식했을 때 추가됨
            _trackedImage = newImage;
        }

        foreach(var newImage in eventArgs.updated)
        {
            if(_obj.activeSelf)
            {
                // 추적되는 오브젝트의 좌표를 계속 가져와 오브젝트의 좌표를 갱신한다
                _obj.transform.position = newImage.transform.position;
                _obj.transform.rotation = newImage.transform.rotation;
            }
        }
    }
}
