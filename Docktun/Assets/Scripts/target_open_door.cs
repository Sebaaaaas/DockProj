using FMODUnity;
using System.Collections;
using UnityEngine;
using TelemetriaDOC;

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
        if ((hittable && other.gameObject.tag == "Sword")|| (hittable && other.gameObject.tag == "Fire"))
        {
            hittable = false;

            Tracker.TrackEvent(new TargetHitEvent(other.gameObject.tag));
            
            eventEmitter.Play();

            

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
