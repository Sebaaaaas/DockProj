using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll_texture : MonoBehaviour
{
    public float scroll_speed_x = 0.5F;
    public float scroll_speed_y = 0.5F;
    public float side_to_side_time = 1.5F;
    float timer = 0F;
    bool forward = false;
    private Vector2 currentOffset;

    private MeshRenderer my_renderer;
    void Start()
    {
        my_renderer = GetComponent<MeshRenderer>();
        currentOffset = Vector2.zero;
    }
    void Update()
    {
        my_renderer.material.mainTextureOffset = new Vector2(scroll_speed_x * Time.realtimeSinceStartup, scroll_speed_y * Time.realtimeSinceStartup);

        timer += Time.deltaTime;
        if(timer > side_to_side_time)
        {
            forward = !forward;
            timer -= side_to_side_time;
        }

        // Aceleramos cuando estamos en medio del movimiento, y movemos en funcion de forwards
        float delta = Time.deltaTime * scroll_speed_x * 0.3f * (forward ? 1 : -1);
        currentOffset.x += delta * (side_to_side_time / 2 - timer);       
        currentOffset.y += delta * (side_to_side_time / 2 - timer); 

        // Acceso al material en el albedo
        my_renderer.material.SetTextureOffset("_DetailAlbedoMap", currentOffset);
    }
}
