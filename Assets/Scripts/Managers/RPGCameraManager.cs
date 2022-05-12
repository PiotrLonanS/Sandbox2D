using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager sharedInstance = null;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
        GameObject vCamGameObject = GameObject.FindWithTag("VirtualCamera");
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }
}

