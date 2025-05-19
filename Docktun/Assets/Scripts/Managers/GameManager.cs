using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TelemetriaDOC;
using DaltonismoHWHAP;




public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<Vector3> tpPoints = new List<Vector3>();

    public string Carpeta_Destino = "Default";


    [Header("True para usar GPU en lugar de CPU")]
    public bool GPU;

    [Header("Filtros de daltonismo")]
    public bool Protanopia;
    public bool Deuteranopia;
    public bool Tritanopia;
    public bool Acromatopsia;

    // Siguiente posicion de la lista a visitar
    int k = 0;

    // Para cambiar la posicion del jugador
    Transform playerTransform;
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

        // Comprobamos si existe un el archivo de posiciones
        bool fileExists = DTMain.readFromFile();

        Debug.Log(fileExists);

        playerTransform = PlayerManager.instance.player.transform;

        // Si existe el archivo, copiamos las posiciones a una lista de lugares a los que queremos hacer un teleport al jugador/camaraa
        if (fileExists)
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            DTMain.addPos(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
            Debug.Log("Pos creada");
            Debug.Log(playerTransform.position.x + "\n" + playerTransform.position.y + "\n" + playerTransform.position.z);
            Debug.Log(DTMain.listSize());
            instance.addToList(playerTransform.position);

            StartCoroutine(instance.captureImage(instance.getTPList().Count));
        }



        if (Input.GetKeyDown(KeyCode.O))
        {
            if (instance.getTPList().Count > 0)
            {
                playerTransform.gameObject.GetComponent<CharacterController>().enabled = false;
                playerTransform.position = instance.getTPpoint(k);

                StartCoroutine(instance.captureImage(k));

                if (k + 1 < instance.getTPList().Count)
                {
                    k++;
                }
                else
                {
                    k = 0;
                }

                playerTransform.gameObject.GetComponent<CharacterController>().enabled = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            DTMain.ClearList();
            instance.clearList();
            k = 0;
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

    public void addToList(Vector3 posit)
    {
        tpPoints.Add(posit);
    }

    public void clearList()
    {
        tpPoints.Clear();
        Debug.Log("Position list cleared");
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
            { "Deuteranopia", this.Deuteranopia },
            { "Tritanopia", this.Tritanopia },
            { "Acromatopia", this.Acromatopsia }
        };

        // Enviar a la DLL

        if (!GPU)        
            DTMain.GenerateImages(pngData, filtros, index, Carpeta_Destino);
        else
        {
            RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 0);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(screenTexture);
            DTMain.GenerateImages(pngData, filtros, index, Carpeta_Destino, screenTexture);
        }
            
    }

}
