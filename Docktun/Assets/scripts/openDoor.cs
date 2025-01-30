using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    public GameObject[] doors;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            animator.Play("Target");
            GetComponent<VCamActivation>().ActivateAttachedCamera();
            for (int i = 0; i < doors.Length; i++)
                doors[i].GetComponent<SlidingDoor>().changeDoorOpen();                  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            animator.Play("Target");
            GetComponent<VCamActivation>().ActivateAttachedCamera();
            for (int i = 0; i < doors.Length; i++)
                doors[i].GetComponent<SlidingDoor>().changeDoorOpen();
        }
    }
}
