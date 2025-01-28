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
    }
    public override void TakeDamage(int damageTaken)
    {
        base.TakeDamage(damageTaken);

        int i = currentHealth;

        if (i <= 0)
            GameManager.instance.OnPlayerDeath();
        else
        {
            while (i > currentHealth - damageTaken && i >= 0)
            {
                hearts[i].sprite = emptyHeart;
                --i;
            }
        }            
    }

    public override void RestoreHealth(int healthRestored)
    {
        base.RestoreHealth(healthRestored);
        
        int i = currentHealth;

        while (i < currentHealth + healthRestored && i <= maxHealth)
        {
            hearts[i - 1].sprite = fullHeart;
            ++i;
        }
    }

}
