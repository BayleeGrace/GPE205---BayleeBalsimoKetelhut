using System.Collections;
using System.Collections.Generic; // this makes sure you can access lists
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> powerups;
    public List<Powerup> removedPowerupQueue;

    void Start()
    {
        //always make sure you initalize a list (below)
        powerups = new List<Powerup>();
        removedPowerupQueue = new List<Powerup>();
    }

    private void Update()
    {
        DecrementPowerupTimers();
    }

    void LateUpdate() // Late Update runs at the END of EVERY frame
    {
        ApplyRemovePowerupsQueue();
    }

    public void Add(Powerup powerupToAdd)
    {
        // TODO: Create the Add method

        powerupToAdd.Apply(this);

        powerups.Add(powerupToAdd);
    }

    public void Remove(Powerup powerupToRemove)
    {
        // TODO: Create the Remove method
        powerupToRemove.Remove(this);

        //Get ready to remove it from the list
        removedPowerupQueue.Add(powerupToRemove);
    }

    public void DecrementPowerupTimers()
    {
        foreach (Powerup powerup in powerups)
        {
            powerup.duration -= Time.deltaTime;

            if (powerup.duration <= 0)
            {
                Remove(powerup);
            }
        }
    }

    private void ApplyRemovePowerupsQueue()
    {
        foreach (Powerup powerup in removedPowerupQueue)
        {
            powerups.Remove(powerup);
        }
        removedPowerupQueue.Clear();
    }
}