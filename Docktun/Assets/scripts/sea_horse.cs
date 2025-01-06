using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class sea_horse : MonoBehaviour
{
    public float rayDistance = 25.0f;
    public float rotationSpeed = 1.0f;
    public Vector3 startPos;

    bool emitFire = false;
    public float timeEmittingFire = 3.0f; // Time emitting fire after seeing player
    float timer;

    // Fireballs
    public ObjectPooler fireballPool;
    public float shootFireballInterval = 1f;
    public GameObject fireballSpawner;

    void Start()
    {
        startPos = transform.position;
        startPos.y += 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0));

        Vector3 direction = transform.forward;

        // Check if player seen, if so set emitting fire to true and start a timer
        if (Physics.Raycast(startPos, direction, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Player") && !emitFire)
            {
                emitFire = true;
                timer = timeEmittingFire;
                StartCoroutine(ShootFireballCoroutine());
            }
        }


        if (emitFire)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                emitFire = false;
            }
        }

        Debug.DrawRay(startPos, direction * rayDistance, Color.red);
    }

    void shootFireball()
    {
        GameObject fireball = fireballPool.GetPooledObject();
        fireball.transform.position = fireballSpawner.transform.position;
        fireball.transform.rotation = transform.rotation;
        fireball.GetComponent<Fireball>().startLifeTimer(); // Delete when lifetime is over

        fireball.SetActive(true);
    }

    IEnumerator ShootFireballCoroutine()
    {
        while (emitFire)
        {
            yield return new WaitForSeconds(shootFireballInterval); // Return if time hasnt passed

            shootFireball();
        }
    }
}
