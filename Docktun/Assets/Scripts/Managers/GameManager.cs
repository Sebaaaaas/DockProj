using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Runtime.InteropServices;
using TelemetriaDOC;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

}
