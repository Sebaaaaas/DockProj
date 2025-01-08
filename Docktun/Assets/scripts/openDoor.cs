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
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Sword")
        {
            Debug.Log("ChangeDoors");
            for(int i = 0; i < doors.Length; i++)
                doors[i].GetComponent<SlidingDoor>().changeDoorOpen();                  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            Debug.Log("ChangeDoors");
            for (int i = 0; i < doors.Length; i++)
                doors[i].GetComponent<SlidingDoor>().changeDoorOpen();
        }
    }
}
