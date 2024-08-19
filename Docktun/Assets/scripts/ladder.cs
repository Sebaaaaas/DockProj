using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladder : MonoBehaviour
{
    public BoxCollider bottom, top;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //if(other.tag == "Player")
        //{
        //    other.GetComponent<player_controller>()
        //}
    }
}
