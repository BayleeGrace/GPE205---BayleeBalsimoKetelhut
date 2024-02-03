using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    // Create a variable that tracks how much damage was dealt
    public float damageDone;
    // Create a variable that holds the data of the owner of this component
    public Pawn owner;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Callback that grabs the collider of the object
    public void OnTriggerEnter(Collider other)
    {
        // Get the Health component from the *Colliding* Game Object
        Health otherHealth = other.gameObject.GetComponent<Health>();
        // What happens when the object doesn't have a Health component? See below
        if (otherHealth != null)
        {
            // In this case, we do have access to a Health component on the colliding object
            // Inflict Damage
            otherHealth.TakeDamage(damageDone, owner);
        }

        // Destroy object
        Destroy(gameObject);
    }
}
