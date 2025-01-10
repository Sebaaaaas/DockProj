using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public bool open;
    bool moving = false;
    public float speed = 2f;

    // How much the door will be moved when opened
    public Vector3 displacement = new Vector3(0, -4.5f, 0);
    // Where the door should be located WHEN CLOSED, IN GLOBAL COORDINATES
    public Vector3 initialPosition;

    public ParticleSystem ps;

    // Open/Close door
    public void changeDoorOpen()
    {
        // Prevent opening/closing while already doing one of them
        if (moving)
        {
            Debug.Log("Cannot move door, animation already playing.");
            return;
        }

        ps.Play();
        StartCoroutine(MoveDoor(open ? initialPosition : initialPosition + displacement));
       
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        moving = true;

        // Smoothly move the door towards the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        transform.position = targetPosition; // Ensure the final position is exact
        moving = false;
        open = !open;
    }
}


