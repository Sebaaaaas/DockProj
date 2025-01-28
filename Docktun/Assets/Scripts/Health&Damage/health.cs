using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;

    bool alive = true;

    private void Start()
    {
        currentHealth = maxHealth;
        alive = true;
    }

    public virtual void TakeDamage(int damageTaken)
    {
        currentHealth-=damageTaken;
        
        if (currentHealth <= 0)
            alive = false;
    }

    public virtual void RestoreHealth(int healthRestored)
    {
        currentHealth += healthRestored;

        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
