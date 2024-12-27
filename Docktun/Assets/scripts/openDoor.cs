using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    public GameObject[] doors;

    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == "Sword")
        {
            for(int i = 0; i < doors.Length; i++)
                doors[i].GetComponent<SlidingDoor>().changeDoorOpen();                  
        }
    }
}
