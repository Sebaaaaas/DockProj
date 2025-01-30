using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamActivation : MonoBehaviour
{
    [SerializeField] float timeActive = 0f; // Time the camera should remain active

    // Index of camera within vcamManager to activate
    [SerializeField] int cameraIndexToActivate;

    // If true, only do the transition once
    [SerializeField] bool once = true;
    public void ActivateAttachedCamera()
    {
        if (once)
        {
            VCamManager.instance.ActivateCamera(cameraIndexToActivate, timeActive);
            once = false;
        }
    }

    public void DeactivateAttachedCamera()
    {
        VCamManager.instance.DeactivateCamera(cameraIndexToActivate);
    }
}
