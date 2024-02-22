using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Variables for tracking current and max health, visible to designers
    #region Variables
    
    [Header("Health Variables")]
    public float maxHealth = 100;
    public float currentHealth;
    public bool canOverheal;
    #endregion Variables

    // Start is called before the first frame update
    private void Start()
    {
        // Sets the currentHealth to maxHealth upon game? start
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    // Specify in TakeDamage that you'll be subtracting an amount FROM the Pawn class that's owning the Health component
    public void TakeDamage(float amount, Pawn source)
    {
        // This means the same as saying "currentHealth = currentHealth - amount;
        currentHealth -= amount;
        // Test code, this will convert all variables to a string for the Debug Log
        if(currentHealth <= 0)
        {
            // The source (Pawn source, the owner of this component) dies (see below)
            Die(source); // argument - the actual value passed into a method call
        }
    }

    public void ReplenishHealth(float amount, Pawn source)
    {
        if (!canOverheal)
        {
        currentHealth += amount;
        // Mathf.Clamp takes the amounts of these variables and creates a clamp between them. This means that currentHealth can't go higher than maxHealth
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
        else
        {
            currentHealth += amount;
        }
    }

    public void Die(Pawn source) // parameter - input to a method
    {
        Destroy(gameObject);
        // source.name takes the "Pawn source" (owner of this component) and collects its name
    }
}
