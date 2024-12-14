using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.healPlayer(1);
            Destroy(gameObject);
        }
    }
}
