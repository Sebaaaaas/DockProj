using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSwitch : MonoBehaviour
{
    // Object that keeps track of each of the switches activated
    public GameObject SwitchGroupManager;

    // Order in which switches must be activated, if it is a fake switch, leave as -1 so it resets counter
    public int index = -1;

    public Texture activeTex, inactiveTex;

    bool active = true;
    private void OnTriggerEnter(Collider other)
    {
        if (active && other.gameObject.tag == "Player")
        {
            SwitchGroupManager.GetComponent<FloorSwitchPuzzle>().receiveSwitchIndex(index);
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", activeTex);
            active = false;
        }
    }

    public void deactivate()
    {
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", inactiveTex);
        active = true;
    }
}
