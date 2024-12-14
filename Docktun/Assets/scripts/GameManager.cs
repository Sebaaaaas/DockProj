using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] int playerHealth = 3;

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this; 
        }
        else if(instance != this){
            Destroy(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void healPlayer(int healthRestored)
    {
        playerHealth += healthRestored;
        Debug.Log(playerHealth);
    }

    void damagePlayer(int damageDealt)
    {
        playerHealth -= damageDealt;
    }
}
