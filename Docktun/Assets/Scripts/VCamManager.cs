using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            cameras.Clear();
            cameras.Add(GameObject.Find("CM vcam2").GetComponent<CinemachineVirtualCamera>()); // Buscar camaras cuando se cargue una nueva escena
            cameras.Add(GameObject.Find("CM vcam3").GetComponent<CinemachineVirtualCamera>()); // Buscar camaras cuando se cargue una nueva escena
        }
    }
    private void OnDestroy()
    {
    
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
