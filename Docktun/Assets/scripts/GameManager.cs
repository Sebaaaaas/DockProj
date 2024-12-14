using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;



    [SerializeField] int playerHealth = 3, maxPlayerHealth = 3;

    public Sprite emptyHeart, fullHeart;

    public Image[] hearts;

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
        // Set the hearts
        for (int i = 0; i < playerHealth; i++)
            hearts[i].sprite = fullHeart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void healPlayer(int healthRestored)
    {
        // First, set the hearts that should be full
        for (int i = playerHealth; i < playerHealth + healthRestored && i < maxPlayerHealth; i++)
            hearts[i].sprite = fullHeart;

        playerHealth += healthRestored;

        if(playerHealth > maxPlayerHealth)
               playerHealth = maxPlayerHealth;

    }

    public void damagePlayer(int damageDealt)
    {
        for (int i = playerHealth; i > playerHealth - damageDealt && i > 0; i--)
            hearts[i - 1].sprite = emptyHeart;

        playerHealth -= damageDealt;

        if(playerHealth < 0)
            playerHealth = 0;

    }
}
