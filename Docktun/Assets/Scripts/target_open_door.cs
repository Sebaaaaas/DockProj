using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target_open_door : MonoBehaviour
{
    public GameObject[] doors;
    [SerializeField] float timeBetweenActivations = 2f;
    bool hittable = true; // If object can be hit
    Animator animator;

    

    StudioEventEmitter eventEmitter;
    void Start()
    {
        animator = GetComponent<Animator>();
        eventEmitter = GetComponent<StudioEventEmitter>();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hittable && other.gameObject.tag == "Sword")
        {
            eventEmitter.Play();

            hittable = false;

            animator.Play("Target");

            GetComponent<VCamActivation>().ActivateAttachedCamera();

            for (int i = 0; i < doors.Length; i++)
                doors[i].GetComponent<SlidingDoor>().changeDoorOpen();

            StartCoroutine(WaitForReactivation());            
        }
    }

    IEnumerator WaitForReactivation()
    {
        float countdown = timeBetweenActivations;
        
        while(countdown > 0)
        {
            countdown-=Time.deltaTime;
            yield return null;
        }
        
        hittable = true;
    }
}
