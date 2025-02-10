using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class wooden_sign : MonoBehaviour
{
    [SerializeField] float timeBetweenActivations = 2f;
    bool hittable = true; // If object can be hit
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hittable && other.gameObject.tag == "Sword")
        {
            hittable = false;
            animator.Play("Cube|sign_boing");

            StartCoroutine(WaitForReactivation());
        }
    }

    IEnumerator WaitForReactivation()
    {
        float countdown = timeBetweenActivations;

        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            yield return null;
        }

        hittable = true;
    }
}
