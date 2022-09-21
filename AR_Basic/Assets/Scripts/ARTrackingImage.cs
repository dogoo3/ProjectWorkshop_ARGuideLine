using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTrackingImage : MonoBehaviour
{
    public float _timer;
    public ARTrackedImageManager trackedImageManager;
    public List<GameObject> _objectList = new List<GameObject>();

    private Dictionary<string, GameObject> _prefabDic = new Dictionary<string, GameObject>();
    private List<ARTrackedImage> _trackedImg = new List<ARTrackedImage>();
    private List<float> _trackedTimer = new List<float>();

    private void Awake()
    {
        foreach (GameObject obj in _objectList)
        {
            string tName = obj.name;
            _prefabDic.Add(tName, obj);
            Debug.Log(string.Format("PREFAB LIST : {0}", tName));
        }
    }

    private void Update()
    {
        if (_trackedImg.Count > 0)
        {
            List<ARTrackedImage> tNumList = new List<ARTrackedImage>();
            for (int i = 0; i < _trackedImg.Count; i++)
            {
                if (_trackedImg[i].trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
                {
                    if (_trackedTimer[i] > _timer)
                    {
                        Debug.Log("3333333333");
                        string name = _trackedImg[i].referenceImage.name;
                        Debug.Log("NAME : " + _trackedImg[i].referenceImage.name);
                        GameObject tObj = _prefabDic[name];
                        tObj.SetActive(false);
                        tNumList.Add(_trackedImg[i]);
                    }
                    else
                    {
                        Debug.Log("4444444444");
                        _trackedTimer[i] += Time.deltaTime;
                    }
                }
            }

            if (tNumList.Count > 0)
            {
                for (int i = 0; i < tNumList.Count; i++)
                {
                    int num = _trackedImg.IndexOf(tNumList[i]);
                    _trackedImg.Remove(_trackedImg[num]);
                    _trackedTimer.Remove(_trackedTimer[num]);
                }
            }
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            if(!_trackedImg.Contains(trackedImage))
            {
                Debug.Log("0000000000");
                _trackedImg.Add(trackedImage);
                _trackedTimer.Add(0);
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if(!_trackedImg.Contains(trackedImage))
            {
                Debug.Log("1111111111");
                _trackedImg.Add(trackedImage);
                _trackedTimer.Add(0);
            }
            else
            {
                Debug.Log("2222222222");
                int num = _trackedImg.IndexOf(trackedImage);
                _trackedTimer[num] = 0;
            }
            UpdateImage(trackedImage);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        GameObject tObj;
        if(_prefabDic.ContainsKey(name))
        {
            tObj = _prefabDic[name];
            Debug.Log("업데이트 이미지 키 있음");
            tObj.transform.position = trackedImage.transform.position;
            tObj.transform.rotation = trackedImage.transform.rotation;
            tObj.SetActive(true);
        }
        else
        {
            Debug.Log("업데이트 이미지 키 없음");
        }
    }
}
