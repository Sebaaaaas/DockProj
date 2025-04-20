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
            Tracker.TrackEvent(new GameStateEvent(Time.realtimeSinceStartup, GameStateEvent.EventType.GameEnd));
            SceneManager.LoadScene("EndScene");
        }
    }
}
