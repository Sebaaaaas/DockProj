using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If object falls into water/void, reset their position
public class water_fall_reset : MonoBehaviour
{
    // If player falls into water/void, we keep track of where we should reset their position
    [SerializeField] float timeBeforePositionReset = 1.5f;
    [SerializeField] float timeBetweenSavingPosition = 1f; // How often we save position
    [SerializeField] float groundRaycastLength = 3f;

    Vector3 lastPosBeforeFall;

    // Start is called before the first frame update
    void Start()
    {
        lastPosBeforeFall = transform.position;

        InvokeRepeating("SaveGroundedPosition", timeBetweenSavingPosition, timeBetweenSavingPosition);
    }

    // Update is called once per frame
    void Update()
    {        
        Debug.DrawRay(transform.position, groundRaycastLength * -transform.up, Color.blue);
    }

    // Save last spot player was grounded
    void SaveGroundedPosition()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, groundRaycastLength))
        {
            if (hit.collider.CompareTag("FallResetLayer"))
                lastPosBeforeFall = transform.position;
        }
    }
    public void ResetToGround()
    {
        StartCoroutine(ResetCoroutine());        
    }
    private IEnumerator ResetCoroutine()
    {
        float startTime = Time.time;

        while (Time.time < startTime + timeBeforePositionReset)
        {
            yield return null;
        }

        // We have to disable the character controller momentarily so it doesnt override the position change
        GetComponent<CharacterController>().enabled = false;
        transform.position = lastPosBeforeFall;
        GetComponent<CharacterController>().enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallResetLayer"))
        {
            ResetToGround();
            GetComponent<health>().TakeDamage(1);
        }
    }
}
