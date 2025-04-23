using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TelemetriaDOC;

public class finishGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Tracker.TrackEvent(new GameStateEvent(GameStateEvent.EventType.GameEnd, GameStateEvent.ResultType.Sucess));
            SceneManager.LoadScene("EndScene");
        }
    }
}
