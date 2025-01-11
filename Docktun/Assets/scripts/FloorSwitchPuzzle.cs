using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitchPuzzle : MonoBehaviour
{
    public GameObject[] switches;
    public GameObject fakeSwitch;

    // Door to open when puzzle is completed
    public GameObject doorToOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        doorToOpen.GetComponent<SlidingDoor>().changeDoorOpen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
