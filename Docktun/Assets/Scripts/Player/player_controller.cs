using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TelemetriaDOC;
using DaltonismoHWHAP;

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
    public GameObject playerSwordHand;

    Animator playerMeshAnimator;
    
    CharacterController character_controller;

    [SerializeField] EventReference bellClang;

    BoxCollider swordAreaCollider; // Its on the player mesh right now

    float timetosend = 3;
    float timepassed = 0;
    int timesSended = 0;
    bool closed = false;

    int k = 0;

    bool tepeado=false;
    

    void Start()
    {        
        character_controller = GetComponent<CharacterController>();

        actions = new Queue<InstantActions>();
        //playerSword.SetActive(false);
        swordAreaCollider = playerMesh.GetComponent<BoxCollider>();

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
            {
                Tracker.TrackEvent(new PlayerAttackEvent());
                StartCoroutine(AttackCoroutine());
            }
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            DTMain.addPos(transform.position.x, transform.position.y, transform.position.z);
            Debug.Log("Pos creada");
            Debug.Log(transform.position.x+"\n"+transform.position.y +"\n"+ transform.position.z);
            Debug.Log(DTMain.listSize());
            GameManager.instance.captureImage();
        }

        if (tepeado)
        {
            Debug.Log("Estoy en: " + transform.position);
            tepeado = false;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = GameManager.instance.getTPpoint(k);
            Debug.Log("Tepea a: " + transform.position);

            if (k + 1 < GameManager.instance.getTPList().Count)
            {

                k++;
            }
            else
            {
                k = 0;
            }
            tepeado = true;
            GetComponent<CharacterController>().enabled = true;
        }

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

        //playerSword.SetActive(true);
        swordAreaCollider.enabled = true;

        //playerMeshAnimator.Play("Armature_001|Attack");

        FMODUnity.RuntimeManager.PlayOneShot(bellClang, transform.position);
        
        playerSwordHand.transform.Rotate(new Vector3(0,120,0));
        while (Time.time < startTime + attack_time)
        {
            yield return null;
        }

        playerSwordHand.transform.Rotate(new Vector3(0, -120, 0));
        //Debug.Log("END ATTACK");
        //playerSword.SetActive(false);
        swordAreaCollider.enabled = false;

        performingInstantAction = false;
    }   
}
