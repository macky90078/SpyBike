using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera m_spyCamera;
    [SerializeField] private Camera m_CGCamera;


    public void ShowSpyCam()
    {
        m_spyCamera.enabled = true;
        m_CGCamera.enabled = false;
    }

    public void ShowCGCam()
    {
        m_spyCamera.enabled = false;
        m_CGCamera.enabled = true;
    }

}
