using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 1.5f;
    public float lifetime = 3f;
    private float remainingLifetime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remainingLifetime -= Time.deltaTime;
        if(remainingLifetime < 0)
        {
            gameObject.SetActive(false);
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void startLifeTimer()
    {
        remainingLifetime = lifetime;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        Debug.Log("Hit");
        if (other.CompareTag("Player"))
        {
            Debug.Log("PlayerHit");
            GameManager.instance.damagePlayer(1);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    gameObject.SetActive(false);
    //    Debug.Log("Hit");
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("PlayerHit");
    //        GameManager.instance.damagePlayer(1);
    //    }
    //}
}
