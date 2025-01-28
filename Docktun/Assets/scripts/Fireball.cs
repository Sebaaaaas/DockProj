using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float speed = 1.5f;
    [SerializeField] float lifetime = 3f;
    [SerializeField] int damage = 1;

    private float remainingLifetime;

    void Update()
    {
        remainingLifetime -= Time.deltaTime;
        if(remainingLifetime < 0)
        {
            gameObject.SetActive(false);
        }

        // Always multiply vectors later than floats for efficiency
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    public void startLifeTimer()
    {
        remainingLifetime = lifetime;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        
        if (other.GetComponent<health>())
        {
            Debug.Log("Hit health-haver");
            other.GetComponent<health>().TakeDamage(damage);
        }
    }
}
