using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{

    //Instance 
    public static PlayerCamera Instance;

    #region Variables
    

    [SerializeField] GameObject defaultCamera;
    [SerializeField] GameObject TopDonwCamera;
    [SerializeField] GameObject BackFacingCamera;

    [SerializeField] GameObject LeftSideForwardCamera;
    [SerializeField] GameObject LeftSideStuntBackCamera;
    [SerializeField] GameObject LeftSideStuntFrontCamera;
    [SerializeField] GameObject LeftSideRightViewCamera45;

    [SerializeField] GameObject RightSideForwardCamera;
    [SerializeField] GameObject RightSideStuntBackCamera;
    [SerializeField] GameObject RightSideStuntFrontCamera;
    [SerializeField] GameObject RightSideLeftViewCamera45;

    #endregion




    IEnumerator ChangeCam(GameObject cam, float time)
    {
        DeactivateAllCams();     
        yield return new WaitForSeconds(0.1f);   
        
       
        cam.GetComponent<CinemachineVirtualCamera>().Priority = 1000;
        if (time > 0)
        {
            yield return new WaitForSeconds(time);
            CallDefaultCam(-1);
        }
        yield return null;
    }
   
    private void Awake()
    {        
        Instance = this;        
    }

    private void Start()
    {
        CallDefaultCam(-1);
        
    }


    #region CamCallMethods

    public void AktivateRandomCamera()
    {
        int random = Random.Range(0, 11);
        
        switch (random)
        {
            case 0:
                CallDefaultCam(2);
                Debug.Log("DefaultCam");
                break;
            case 1:
                CallTopDownCam(2);
                Debug.Log("TopDownCam");
                break;
            case 2:
                CallBackFacingCam(2);
                Debug.Log("BackFacingCam");
                break;
            case 3:
                CallLeftSideForwardCam(2);
                Debug.Log("LeftSideForwardCam");
                break;
            case 4:
                CallLeftSideStuntBackCam(2);
                Debug.Log("LeftSideStuntBackCam");
                break;
            case 5:
                CallLeftSideStuntFrontCam(2);
                Debug.Log("LeftSideStuntFrontCam");
                break;
            case 6:
                CallRightSideForwardCam(2);
                Debug.Log("RightSideForwardCam");
                break;
            case 7:
                CallRightSideStuntBackCam(2);
                Debug.Log("RightSideStuntBackCam");
                break;
            case 8:
                CallRightSideStuntFrontCam(2);
                Debug.Log("RightSideStuntFrontCam");
                break;
            case 9:
                CallLeftSideRightViewCam45(2);
                Debug.Log("LeftSideRightViewCam45");
                break;
            case 10:
                CallRightSideLeftViewCam45(2);
                Debug.Log("RightSideLeftViewCam45");
                break;
            default:
                CallDefaultCam(-1);
                Debug.Log("DefaultCam");
                break;
        }
    }
    public void CallDefaultCam(float time)
    {        
        StartCoroutine(ChangeCam(defaultCamera, time));
    }

    public void CallTopDownCam(float time)
    {        
        StartCoroutine(ChangeCam(TopDonwCamera, time));
    }

    public void CallBackFacingCam(float time)
    {
        
        StartCoroutine(ChangeCam(BackFacingCamera, time));
    }

    public void CallLeftSideForwardCam(float time)
    {
        
        StartCoroutine(ChangeCam(LeftSideForwardCamera, time));
    }

    public void CallLeftSideStuntBackCam(float time)
    {
        
        StartCoroutine(ChangeCam(LeftSideStuntBackCamera, time));
    }

    public void CallLeftSideStuntFrontCam(float time)
    {
        
        StartCoroutine(ChangeCam(LeftSideStuntFrontCamera, time));
    }

    public void CallRightSideForwardCam(float time)
    {
        
        StartCoroutine(ChangeCam(RightSideForwardCamera, time));
    }

    public void CallRightSideStuntBackCam(float time)
    {
        
        StartCoroutine(ChangeCam(RightSideStuntBackCamera, time));
    }

    public void CallRightSideStuntFrontCam(float time)
    {
        
        StartCoroutine(ChangeCam(RightSideStuntFrontCamera, time));
    }

    public void CallLeftSideRightViewCam45(float time)
    {
       
        StartCoroutine(ChangeCam(LeftSideRightViewCamera45, time));
    }

    public void CallRightSideLeftViewCam45(float time)
    {
        
        StartCoroutine(ChangeCam(RightSideLeftViewCamera45, time));
    }



    private void DeactivateAllCams()
    {
        
        defaultCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        TopDonwCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        BackFacingCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        LeftSideForwardCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        LeftSideStuntBackCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        LeftSideStuntFrontCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        LeftSideRightViewCamera45.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        RightSideForwardCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        RightSideStuntBackCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        RightSideStuntFrontCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        RightSideLeftViewCamera45.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        


           
    }

    #endregion

}
