using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll_texture : MonoBehaviour
{
    public float scroll_speed_x = 0.5F;
    public float scroll_speed_y = 0.5F;

    private MeshRenderer my_renderer;
    void Start()
    {
        my_renderer = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        my_renderer.material.mainTextureOffset = new Vector2(scroll_speed_x * Time.realtimeSinceStartup, scroll_speed_y * Time.realtimeSinceStartup);
    }
}
