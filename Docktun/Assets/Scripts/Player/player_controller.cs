using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    // Possible actions by player that arent instant(moving would be instant)
    enum InstantActions {ATTACK }
    Queue<InstantActions> actions;
    InstantActions currentAction;
    float time_clear_action_queue = 0.5f, current_time_clear_Action_queue;

    // We cannot perform any action, instant or not, while we are performing an action
    bool performingInstantAction = false;

    [SerializeField] private KeyCode attack_key = KeyCode.Joystick1Button5;
    [SerializeField] private KeyCode run_key = KeyCode.Joystick1Button4;

    public float gravity = -9.8f;

    [SerializeField] private float speed = 10.0f, attack_time = 0.2f, runSpeed = 14.0f;
    bool running = false;

    public GameObject playerMesh;
    public GameObject playerSword;

    Animator playerMeshAnimator;
    
    CharacterController character_controller;

    [SerializeField] EventReference bellClang;

    void Start()
    {        
        character_controller = GetComponent<CharacterController>();

        actions = new Queue<InstantActions>();
        playerSword.SetActive(false);

        playerMeshAnimator = playerMesh.GetComponent<Animator>();
    }

    void Update()
    {

        // Apply gravity
        character_controller.Move(new Vector3(0f, gravity, 0f) * Time.deltaTime);

        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (direction != Vector3.zero) // If moving
        {
            playerMeshAnimator.SetFloat("speed", 1);
        }
        else
        {
            playerMeshAnimator.SetFloat("speed", 0);
        }

        // Change player direction
        turn(direction);

        // Check for instant actions, add to actions queue
        if (Input.GetKeyDown(attack_key))
            actions.Enqueue(InstantActions.ATTACK);

        if (Input.GetKey(run_key))
            running = true;
        else
            running = false;

        // If available, get next action to perform
        if (!performingInstantAction && actions.Count != 0)
        {
            //string s = "";
            //foreach (var action in actions)
            //    s += action + " - ";

            //Debug.Log(s);

            currentAction = actions.Dequeue();
            performingInstantAction = true;

            if(currentAction == InstantActions.ATTACK)
                StartCoroutine(AttackCoroutine());
        }

        // Reset action quere timer
        if(actions.Count == 0)
        {
            current_time_clear_Action_queue = time_clear_action_queue;
        }
        else
        {
            current_time_clear_Action_queue -= Time.deltaTime;

            if(current_time_clear_Action_queue <= .0f)
                actions.Clear();
        }        

        // Move player
        if (!performingInstantAction)
            move(direction);
    }

    private void turn(Vector3 direction)
    {       
        if (direction != Vector3.zero)
        {
            playerMesh.transform.forward = direction;
        }
    }
    private void move(Vector3 direction)
    {        
        if (direction.magnitude >= 0.1f)
        {
            if(running)
                character_controller.Move(direction * runSpeed * Time.deltaTime);
            else
                character_controller.Move(direction * speed * Time.deltaTime);

        }
    }
    private IEnumerator AttackCoroutine()
    {
        //Debug.Log("ATTACK");
        float startTime = Time.time;
        
        playerSword.SetActive(true);

        FMODUnity.RuntimeManager.PlayOneShot(bellClang, transform.position);

        while (Time.time < startTime + attack_time)
        {
            yield return null;
        }

        //Debug.Log("END ATTACK");
        playerSword.SetActive(false);

        performingInstantAction = false;
    }   
}
