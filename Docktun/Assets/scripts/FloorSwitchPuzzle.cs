using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitchPuzzle : MonoBehaviour
{
    // REAL switches involved in puzzle(do not include "fake" switches)
    public List<GameObject> switches;

    // Door to open when puzzle is completed
    public GameObject doorToOpen;

    // Index of next switch which must be activated
    int switchIndex = 0;

    public void receiveSwitchIndex(int index)
    {
        if (index == switchIndex) // Correct activation
        {
            switchIndex++;

            if(switchIndex == switches.Count)
                doorToOpen.GetComponent<SlidingDoor>().changeDoorOpen();
        }
        else // Incorrect activation, restar count
        {
            switchIndex = 0;
        }
    }
}
