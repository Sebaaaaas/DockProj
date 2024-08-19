using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    public Transform cam;
    public float gravity = -9.8f;
    public KeyCode run_key = KeyCode.LeftShift;
    public float speed = 10.0f, walk_speed = 10.0f, run_speed = 12.0f, climb_speed = 4.0f;
    public float turn_smooth_time = 0.1f;

    enum STATE { WALKING, RUNNING, CLIMBING, FALLING };
    STATE current_state = STATE.WALKING;
    
    CharacterController character_controller;
    float turn_smooth_velocity;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        character_controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        character_controller.Move(new Vector3(0f,gravity,0f) * Time.deltaTime);

        Vector3 direction = Vector3.zero;

        if (current_state == STATE.WALKING)
            direction = walking_state();
        else if (current_state == STATE.CLIMBING)
            direction = climbing_state();

        if (direction.magnitude >= 0.1f)
        {
            float target_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_angle, ref turn_smooth_velocity, turn_smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move_direction = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;

            character_controller.Move(move_direction.normalized * speed * Time.deltaTime);
        }
    }

    Vector3 walking_state()
    {
        SetSpeed();

        // Move the player
        return new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
    }
    Vector3 climbing_state()
    {
        speed = climb_speed;
        return new Vector3(0f, Input.GetAxis("Vertical"), 0f).normalized;
    }
    void SetSpeed()
    {
        if (Input.GetKey(run_key))
        {
            speed = run_speed;
        }
        else
        {
            speed = walk_speed;
        }
    }
}
