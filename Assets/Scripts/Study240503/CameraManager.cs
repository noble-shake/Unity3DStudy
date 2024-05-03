using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] List<GameObject> listCamObj;
    [SerializeField] List<Button> listBtns;


    public enum eCameras
    {
        MainCam,
        Cam2,
        Cam3,

    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void InitBtsn() {
        int count = listBtns.Count;
        for (int inum = 0; inum < listCamObj.Count; inum++)
        {
            // lambada가 inum을 기억해서 레퍼런스화 시킨다. for 문 안에서 주의
            //listBtns[inum].onClick.AddListener(() => switchCamera(inum));

            int value = inum;
            listBtns[inum].onClick.AddListener(() => switchCamera(value));

        }
    }

    private void switchCamera(int _cam)
    {
        int Count = listCamObj.Count;

        for (int inum = 0; inum < listCamObj.Count; inum++)
        {

            listCamObj[inum].SetActive(inum == _cam);

        }
    }


    private void switchCamera(eCameras _cam) {
        int Count = listCamObj.Count;

        for (int inum = 0; inum < listCamObj.Count; inum++) {

            listCamObj[inum].SetActive(inum == (int)_cam);

        }
    }

    void Start()
    {
        switchCamera(eCameras.MainCam);
        InitBtsn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            switchCamera(eCameras.MainCam);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switchCamera(eCameras.Cam2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            switchCamera(eCameras.Cam3);
        }
    }
}
