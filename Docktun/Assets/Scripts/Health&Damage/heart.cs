using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<player_health>())
        {
            other.gameObject.GetComponent<player_health>().RestoreHealth(1);
            Destroy(gameObject);
        }
    }
}
