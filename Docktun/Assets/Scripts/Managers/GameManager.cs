using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Runtime.InteropServices;
using TelemetriaDOC;
using DaltonismoHWHAP;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Vector3> tpPoints = new List<Vector3>();
    public ComputeShader colorblindnessFilters;

    private void Awake()
    {
       
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }

        Tracker.Init(Format.JSON, Type.Disk, "TrackedEvents", 20, 5000);

        Tracker.TrackEvent(new SessionEvent(SessionEvent.EventType.SessionStart));
        Tracker.TrackEvent(new GameStateEvent(GameStateEvent.EventType.GameStart, GameStateEvent.ResultType.Sucess));

        DTMain.Init();
    }
    
    void Start()
    {
       

        //Cursor.lockState = CursorLockMode.Locked;

        //DTMain.captureScreen();
        StartCoroutine(captureImage());

        bool lee = DTMain.readFromFile();
        Debug.Log(lee);
        if (lee)
        {
            int tam = DTMain.listSize();
            for (int i = 0; i < tam; i++)
            {
                tpPoints.Add(new Vector3(DTMain.returnValOfList(i)._x, DTMain.returnValOfList(i)._y, DTMain.returnValOfList(i)._z));
            }
            Debug.Log(tpPoints.Count);
        }
        
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 0);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(screenTexture);
            DTMain.SetColorBlindnessComputeShaders(colorblindnessFilters);
            DTMain.ProcessImageOnGPU(screenTexture);
            Debug.Log("GPU");
        }
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Ded");
        SceneManager.LoadScene("GameOver");
    }

    private void OnApplicationQuit()
    {
        Tracker.TrackEvent(new GameStateEvent(GameStateEvent.EventType.GameEnd, GameStateEvent.ResultType.Quit));
        Tracker.TrackEvent(new SessionEvent(SessionEvent.EventType.SessionEnd));
        Tracker.Closing();
        DTMain.writeToFile();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            Tracker.TrackEvent(new Puzzle1StartEvent());
        }
    }

    public List<Vector3> getTPList()
    {
        return tpPoints;
    }

    public Vector3 getTPpoint(int index)
    {
        return tpPoints[index];
    }

    public System.Collections.IEnumerator captureImage()
    {
        yield return new WaitForEndOfFrame();

        // Capturar pantalla del juego
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Convertir a PNG (byte[])
        byte[] pngData = tex.EncodeToPNG();
        Destroy(tex); // liberar memoria

        // Enviar a la DLL
        DTMain.captureScreen(pngData, pngData.Length);
    }
}
