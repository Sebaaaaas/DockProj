using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    public GameObject door;

    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == "Sword")
        {
            door.GetComponent<SlidingDoor>().changeDoorOpen();                  
        }
    }
}
