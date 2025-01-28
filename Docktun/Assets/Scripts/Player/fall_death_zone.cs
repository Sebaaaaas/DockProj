using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fall_death_zone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<player_controller>())
        {
            other.GetComponent<player_controller>().resetToSolidGround();
            other.GetComponent<player_health>().TakeDamage(1);
        }

    }

}
