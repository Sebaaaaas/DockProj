using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TelemetriaDOC;

public class ButtonEndScene : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void changeScene()
    {
        Tracker.TrackEvent(new GameStateEvent(Time.realtimeSinceStartup,GameStateEvent.EventType.GameStart, GameStateEvent.ResultType.Sucess));
        Tracker.TrackEvent(new Puzzle1StartEvent(Time.realtimeSinceStartup/*, GetGameID()*/));
        //Cambiar el ID del juego

        SceneManager.LoadScene("SampleScene");
    }
}
