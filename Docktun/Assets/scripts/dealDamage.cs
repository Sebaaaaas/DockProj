using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script added to all gameobjects that should deal damage when colliding with the player
public class dealDamage : MonoBehaviour
{
    // How much damage is dealt
    public int damage;


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Player")
    //    {
    //        GameManager.instance.damagePlayer(damage);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.damagePlayer(damage);
        }
    }
}
