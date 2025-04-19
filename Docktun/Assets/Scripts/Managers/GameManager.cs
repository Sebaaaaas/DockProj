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
        Debug.Log(TelemetriaDOC.Tracker.Init());

        Tracker.TrackEvent(new SessionEvent(Time.deltaTime, SessionEvent.EventType.SessionStart));

        DontDestroyOnLoad(instance);
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Tracker.TrackEvent(new Puzzle1StartEvent(Time.realtimeSinceStartup, GetGameID()));

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

}
