using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit_hole : MonoBehaviour
{
    // The tile which will get its collider deactivated so player falls when touching hole
    public GameObject exitFloor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            exitFloor.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
