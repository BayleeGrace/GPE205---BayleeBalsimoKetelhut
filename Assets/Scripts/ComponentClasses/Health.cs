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
    public GameObject scorePickup;
    #endregion Variables
    #region Sound variables
    public AudioSource damageTakenAudioSource;
    public AudioClip damageTakenAudioClip;
    #endregion Sound variables

    // Start is called before the first frame update
    private void Start()
    {
        // Sets the currentHealth to maxHealth upon game? start
        currentHealth = maxHealth;

    }

    // Specify in TakeDamage that you'll be subtracting an amount FROM the Pawn class that's owning the Health component
    public void TakeDamage(float amount, Pawn source)
    {
        PlayTakeDamageSound();
        // This means the same as saying "currentHealth = currentHealth - amount;
        currentHealth -= amount;
        // Test code, this will convert all variables to a string for the Debug Log
        if(currentHealth <= 0)
        {

            Pawn pawn = gameObject.GetComponent<Pawn>();

            if (pawn.isPlayer == true)
            {
                pawn.currentLives -= 1;
                Debug.Log("Player lives " + "= " + pawn.currentLives);
                if (pawn.currentLives <= 0)
                {
                    GameManager.instance.ActivateGameOverScreen();
                }
                else 
                {
                    GameManager.instance.OnPlayerDeath(pawn);
                }
                Die(source);
            }
            else if (pawn.isPlayer == false)
            {
                Die(source);
            }
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
        GameObject newScorePickup = Instantiate(scorePickup, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        // source.name takes the "Pawn source" (owner of this component) and collects its name
    }

    public void PlayTakeDamageSound()
    {
        if (!damageTakenAudioSource.isPlaying)
        {
            damageTakenAudioSource.PlayOneShot(damageTakenAudioClip);
        }
    }
}
