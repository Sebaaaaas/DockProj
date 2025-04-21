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

    System.Guid gameID;

    private void Awake()
    {

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this; 
        }
        else if(instance != this){
            Destroy(gameObject);
        }

        //Debug.Log(TelemetriaDOC.Tracker.Number(2,3));
        gameID = System.Guid.NewGuid();
        Debug.Log(TelemetriaDOC.Tracker.Init(Format.JSON,Type.Disk,"TrackedEvents",3));

        Tracker.TrackEvent(new SessionEvent(Time.realtimeSinceStartup, SessionEvent.EventType.SessionStart));
        Tracker.TrackEvent(new GameStateEvent(Time.realtimeSinceStartup, GameStateEvent.EventType.GameStart, GameStateEvent.ResultType.Sucess));


        DontDestroyOnLoad(instance);
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Tracker.TrackEvent(new Puzzle1StartEvent(Time.realtimeSinceStartup/*, GetGameID()*/));

    }

    public System.Guid GetGameID()
    {
        return gameID;
    }

    private void Update()
    {
        
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
        Tracker.closing();
    }

}
