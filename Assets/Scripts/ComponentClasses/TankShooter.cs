using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    public Transform firepointTransform;
    public GameObject tankShellPrefab;
    
    // Start is called before the first frame update
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public override void Shoot(GameObject tankShellPrefab, float fireForce, float damageDone, float lifespan)
    {
        // Instantiate our projectile
        GameObject newShell = Instantiate(tankShellPrefab, firepointTransform.position, firepointTransform.rotation) as GameObject;
        // Grab the "DamageOnHit" component
        DamageOnHit damageOnHit = newShell.AddComponent<DamageOnHit>();

        // If it has one
        if (damageOnHit != null)
        {
            damageOnHit.damageDone = damageDone;

            damageOnHit.owner = GetComponent<Pawn>();
        }

        // Get the rigidbody component
        Rigidbody rigidbody = newShell.GetComponent<Rigidbody>();

        if (rigidbody != null)
        {
            rigidbody.AddForce(firepointTransform.forward * fireForce);
        }
        Destroy(newShell, lifespan);
        
    }
}
