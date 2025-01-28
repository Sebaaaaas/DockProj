using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_collider : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<health>())
        {
            other.gameObject.GetComponent<health>().TakeDamage(damage);
        }
    }
}
