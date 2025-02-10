using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class sea_horse : MonoBehaviour
{
    [SerializeField] float rayDistance = 25.0f;
    [SerializeField] float initialRotationSpeed = 1.0f;
    private float rotationSpeed;

    public Vector3 startPos;

    bool emitFire = false, preheating = false;
    public float timeEmittingFire = 3.0f, timePreheating = 0.5f; // Time emitting fire after seeing player and
                                                                 // time to start firing upon seeing player
    float timer;

    // Fireballs
    public ObjectPooler fireballPool;
    public float shootFireballInterval = 1f;
    public GameObject fireballSpawner;

    // Smoke puffs
    public ParticleSystem smokeParticleSystem;

    // For FMOD fireball sound
    StudioEventEmitter eventEmitter;

    GameObject player;
    void Start()
    {
        startPos = transform.position;
        startPos.y += 3.0f;

        rotationSpeed = initialRotationSpeed;

        eventEmitter = GetComponent<StudioEventEmitter>();

        player = PlayerManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));              

        if (emitFire)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                emitFire = false;
            }
        }        
    }

    private void FixedUpdate()
    {
        Vector3 direction = transform.forward;

        // Check if player is nearby and seen, if so set emitting fire to true and start a timer
        if ((transform.position - player.transform.position).magnitude < rayDistance &&
            Physics.Raycast(startPos, direction, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Player") && !preheating && !emitFire)
            {
                timer = timePreheating;
                StartCoroutine(PreheatAnimationCoroutine());
            }
        }

        if ((transform.position - player.transform.position).magnitude < rayDistance)
            Debug.DrawRay(startPos, direction * rayDistance, Color.red);
    }

    IEnumerator PreheatAnimationCoroutine()
    {
        preheating = true;
        rotationSpeed = .0f;

        // Save initial position
        Vector3 initialPosition = transform.position;
        Vector3 targetPositionUp = initialPosition + new Vector3(0, 1f, 0);
        float moveUpSpeed = 0.5f;

        while (timer > 0)
        {
            // Move up while preheating
            transform.position = Vector3.Lerp(transform.position, targetPositionUp, moveUpSpeed * Time.deltaTime);

            timer -= Time.deltaTime;
            yield return null;
        }

        preheating = false;
        rotationSpeed = initialRotationSpeed;
        timer = timeEmittingFire;
        emitFire = true;
        smokeParticleSystem.Play();

        // Move the object down quickly
        float moveDownSpeed = 5f;
        while (transform.position.y > initialPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveDownSpeed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(ShootFireballCoroutine());
    }
    void shootFireball()
    {
        GameObject fireball = fireballPool.GetPooledObject();
        fireball.transform.position = fireballSpawner.transform.position;
        fireball.transform.rotation = transform.rotation;
        fireball.GetComponent<Fireball>().startLifeTimer(); // Delete when lifetime is over
        eventEmitter.Play();


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
