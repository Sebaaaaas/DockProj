using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class VCamManager : MonoBehaviour
{
    public static VCamManager instance;

    public List<CinemachineVirtualCamera> cameras;

    PauseManager pauseManager;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        pauseManager = PauseManager.instance;
    }

    // Activate camera, if positive value is given for timeActive, deactivate after timeActive seconds
    public void ActivateCamera(int cameraIndex, float timeActive = 0)
    {
        if(cameraIndex < 0 || cameraIndex > cameras.Count)
        {
            Debug.Log("Index out of bounds in camera activated");
        }

        cameras[cameraIndex].Priority = 11;

        if(timeActive > 0)
            StartCoroutine(DeactivateInTime(timeActive, cameraIndex));
        
    }

    public void DeactivateCamera(int cameraIndex)
    {
        if (cameraIndex < 0 || cameraIndex > cameras.Count)
        {
            Debug.Log("Index out of bounds in camera activated");
        }

        cameras[cameraIndex].Priority = 9;
    }    

    IEnumerator DeactivateInTime(float timeActive, int cameraIndex)
    {
        pauseManager.Pause();
        yield return new WaitForSecondsRealtime(timeActive);
        DeactivateCamera(cameraIndex);
        pauseManager.Unpause();
    }
}
