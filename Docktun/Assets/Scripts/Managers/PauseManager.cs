using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    
    private List<GameObject> pauseGameObjects = new List<GameObject>(); // In reality, well disable all these objects while were paused
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Gameobjects with the Pause component will add themselves automatically
    public void AddToPause(GameObject gameObject)
    {
        pauseGameObjects.Add(gameObject);
    }

    public void Pause()
    {
        foreach(GameObject g in pauseGameObjects)
        {
            g.SetActive(false);
        }
    }

    public void Unpause()
    {
        foreach (GameObject g in pauseGameObjects)
        {
            g.SetActive(true);
        }
    }
}
