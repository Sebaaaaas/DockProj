using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dealDamage : MonoBehaviour
{
    // How much damage is dealt
    public int damage;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.GetComponent<health>())
        {
            other.gameObject.GetComponent<health>().TakeDamage(damage);
        }
    }
}
