using System.Collections;
using System.Collections.Generic;
using TelemetriaDOC;
using TelemetriaDOC.Events;
using UnityEngine;

public class FloorSwitchPuzzle : MonoBehaviour
{
    // Switches involved in puzzle
    public List<GameObject> switches;

    // Index of the final switch, be sure to have them as 0,1,2,...,x and declare this as x
    public int finalSwitchIndex;

    // Door to open when puzzle is completed
    public GameObject doorToOpen;

    // Index of next switch which must be activated
    int switchIndex = 0;

    float resetPuzzleTimer = 1.5f;
    public void receiveSwitchIndex(int index)
    {
        if (index == switchIndex) // Correct activation
        {
            switchIndex++;
            
            if(switchIndex == finalSwitchIndex + 1)
            {
                GetComponent<VCamActivation>().ActivateAttachedCamera();
                doorToOpen.GetComponent<SlidingDoor>().changeDoorOpen();
                Tracker.TrackEvent(new Puzzle2SuccessEvent(Time.realtimeSinceStartup));
                Tracker.TrackEvent(new Puzzle2EndEvent(Time.realtimeSinceStartup/*, GameManager.instance.GetGameID()*/));
            }
        }
        else // Incorrect activation, restart count
        {
            StartCoroutine(ResetPuzzleCoroutine());
            switchIndex = 0;            
        }
    }

    private IEnumerator ResetPuzzleCoroutine()
    {
        Tracker.TrackEvent(new Puzzle2ResetEvent(Time.realtimeSinceStartup));
        float timer = resetPuzzleTimer; // Wait before resetting puzzle
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        for (int i = 0; i < switches.Count; ++i) 
            switches[i].GetComponent<FloorSwitch>().deactivate();

    }
}
