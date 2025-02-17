using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{    
    PauseManager pauseManager;
    void Start()
    {
        PauseManager.instance.AddToPause(gameObject);
    }
}
