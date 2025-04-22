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

        Tracker.Init(Format.JSON, Type.Disk, "TrackedEvents", 20);


        Tracker.TrackEvent(new SessionEvent(Time.realtimeSinceStartup, SessionEvent.EventType.SessionStart));
        Tracker.TrackEvent(new GameStateEvent(Time.realtimeSinceStartup, GameStateEvent.EventType.GameStart, GameStateEvent.ResultType.Sucess));
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Tracker.TrackEvent(new Puzzle1StartEvent(Time.realtimeSinceStartup));

    }

    public void OnPlayerDeath()
    {
        Debug.Log("Ded");
        SceneManager.LoadScene("GameOver");
        //TelemetriaDOC.Tracker.
    }

    private void OnApplicationQuit()
    {
        Tracker.TrackEvent(new GameStateEvent(Time.realtimeSinceStartup, GameStateEvent.EventType.GameEnd, GameStateEvent.ResultType.Quit));
        Tracker.TrackEvent(new SessionEvent(Time.realtimeSinceStartup, SessionEvent.EventType.SessionEnd));
        Tracker.Closing();
    }

}
