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

    public float gravedadDalt = 1;


    [Header("Filtros de daltonismo")]
    public bool Protanopia;
    public bool Protanomalia;
    public bool Deuteranopia;
    public bool Deuteranomalia;
    public bool Tritanopia;
    public bool Tritanomalia;
    public bool Acromatopia;
    public bool Acromatomalia;



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

        DTMain.Init(gravedadDalt);
    }
    
    void Start()
    {
       
        //Cursor.lockState = CursorLockMode.Locked;

        //StartCoroutine(captureImage());

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
            for (int i = 0; i < 8; i++) DTMain.ProcessImageOnGPU(screenTexture, i);
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

    public System.Collections.IEnumerator captureImage(int index)
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

        //decidir que filtros se van a usar
        Dictionary<string, bool> filtros = new()
        {
            { "Protanopia", this.Protanopia },
            { "Protanomalia", this.Protanomalia },
            { "Deuteranopia", this.Deuteranopia },
            { "Deuteranomalia", this.Deuteranomalia },
            { "Tritanopia", this.Tritanopia },
            { "Tritanomalia", this.Tritanomalia },
            { "Acromatopia", this.Acromatopia },
            { "Acromatomalia", this.Acromatomalia }
        };


        // Enviar a la DLL
        DTMain.captureScreen(pngData, pngData.Length, filtros, index);
    }

    public System.Collections.IEnumerator CaptureAfterTeleport(int k)
    {
        // Esperar al siguiente frame completo
        yield return null;
        yield return new WaitForEndOfFrame(); 
        yield return StartCoroutine(GameManager.instance.captureImage(k));
    }
}
