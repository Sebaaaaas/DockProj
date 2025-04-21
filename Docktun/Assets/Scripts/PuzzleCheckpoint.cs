using System.Collections;
using System.Collections.Generic;
using TelemetriaDOC;
using UnityEngine;

public class PuzzleCheckpoint : MonoBehaviour
{
    bool triggered = false;

    private void Start()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggered)
        {
            triggered = true;
            Tracker.TrackEvent(new Puzzle1EndEvent(Time.realtimeSinceStartup/*, GameManager.instance.GetGameID()*/));
            Tracker.TrackEvent(new Puzzle2StartEvent(Time.realtimeSinceStartup/*, GameManager.instance.GetGameID()*/));
        }
    }
}
