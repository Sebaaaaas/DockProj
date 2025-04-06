using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Runtime.InteropServices;
using TelemetriaDOC;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    

    private void Awake()
    {

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this; 
        }
        else if(instance != this){
            Destroy(gameObject);
        }

        Debug.Log(TelemetriaDOC.Class1.Multiply(2, 3));

        DontDestroyOnLoad(instance);
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Ded");
        SceneManager.LoadScene("GameOver");
    }

}
