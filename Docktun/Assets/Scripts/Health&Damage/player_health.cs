using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class player_health : health
{
    [SerializeField] Sprite emptyHeart, fullHeart;
    [SerializeField] GameObject heartContainer;
    [SerializeField] GameObject heart; // First and leftmost heart, rest will be cloned from this one
    [SerializeField] float horizontalHeartDistance = 45f;

    // For easy access to the heart icons
    List<Image> hearts = new List<Image>();

    Invulnerability playerInvulnerability;

    private void Start()
    {
        currentHealth = maxHealth;

        hearts.Add(heart.GetComponent<Image>());
        Vector3 startPosition = heart.transform.position;

        GameObject newHeart;

        for (int i = 1; i < maxHealth; ++i)
        {
            newHeart = Instantiate(heart, new Vector3(startPosition.x + i * horizontalHeartDistance, startPosition.y, startPosition.z), Quaternion.identity, heartContainer.transform);

            hearts.Add(newHeart.GetComponent<Image>());

            if (i >= currentHealth)
                hearts[i].sprite = emptyHeart;
        }

        playerInvulnerability = GetComponent<Invulnerability>();
    }
    public override void TakeDamage(int damageTaken)
    {
        if (!playerInvulnerability.getInvulnerable())
        {
            base.TakeDamage(damageTaken);

            GetComponent<Invulnerability>().StartInvulnerability();

            for(int i = maxHealth - 1; i >= currentHealth; --i)
                hearts[i].sprite = emptyHeart;

            if (currentHealth <= 0)
                GameManager.instance.OnPlayerDeath();            
        }
    }

    public override void RestoreHealth(int healthRestored)
    {
        base.RestoreHealth(healthRestored);        

        for(int i = 0; i < currentHealth; ++i)
            hearts[i].sprite = fullHeart;
 
    }
}
