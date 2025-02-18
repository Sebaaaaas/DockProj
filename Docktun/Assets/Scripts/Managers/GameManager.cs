using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Ded");
    }

}
