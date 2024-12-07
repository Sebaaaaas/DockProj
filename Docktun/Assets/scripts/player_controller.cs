using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    // Possible actions by player that arent instant(moving would be instant)
    enum InstantActions {ATTACK, DASH }
    Queue<InstantActions> actions;
    InstantActions currentAction;
    float time_clear_action_queue = 0.5f, current_time_clear_Action_queue;
    // We cannot perform any action, instant or not, while we are performing an action
    bool performingInstantAction = false;

    [SerializeField] private KeyCode dash_key = KeyCode.Joystick1Button8;
    [SerializeField] private KeyCode attack_key = KeyCode.Joystick1Button9;

    public float gravity = -9.8f;

    [SerializeField] private float speed = 10.0f, dash_time = 0.1f, dash_speed = 0.2f, attack_time = 0.2f;
    public GameObject playerMesh;
    public GameObject playerSword;
    
    CharacterController character_controller;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        character_controller = GetComponent<CharacterController>();

        actions = new Queue<InstantActions>();
        playerSword.SetActive(false);
    }

    void Update()
    {
        for (int i = 0; i <= 19; i++) // Check up to 20 buttons
        {
            if (Input.GetKeyDown((KeyCode)(KeyCode.Joystick1Button0 + i)))
            {
                Debug.Log("Button " + i + " pressed");
            }
        }
        // Apply gravity
        character_controller.Move(new Vector3(0f, gravity, 0f) * Time.deltaTime);

        Vector3 direction = Vector3.zero;

        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // Change player direction
        turn(direction);

        // Check for instant actions, add to actions queue
        if (Input.GetKeyDown(dash_key))
            actions.Enqueue(InstantActions.DASH);
        if(Input.GetKeyDown(attack_key))
            actions.Enqueue(InstantActions.ATTACK);

        // If available, get next action to perform
        if (!performingInstantAction && actions.Count != 0)
        {
            //string s = "";
            //foreach (var action in actions)
            //    s += action + " - ";

            //Debug.Log(s);

            currentAction = actions.Dequeue();
            performingInstantAction = true;

            if (currentAction == InstantActions.DASH)
                StartCoroutine(DashCoroutine());
            else if(currentAction == InstantActions.ATTACK)
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
        if(!performingInstantAction)
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
            character_controller.Move(direction * speed * Time.deltaTime);
        }
    }

    private IEnumerator DashCoroutine()
    {
        Debug.Log("DASH");
        float startTime = Time.time;
        
        while (Time.time < startTime + dash_time)
        {
            transform.Translate(playerMesh.transform.forward * dash_speed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("END DASH");
        performingInstantAction = false;
    }

    private IEnumerator AttackCoroutine()
    {
        Debug.Log("ATTACK");
        float startTime = Time.time;
        
        playerSword.SetActive(true);

        while (Time.time < startTime + attack_time)
        {
            yield return null;
        }

        Debug.Log("END ATTACK");
        playerSword.SetActive(false);

        performingInstantAction = false;
    }
}
