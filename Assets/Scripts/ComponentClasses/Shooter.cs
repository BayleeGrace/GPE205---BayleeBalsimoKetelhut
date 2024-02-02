using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    public abstract void Start();

    public abstract void Update();

    public virtual void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifespan)
    {
        throw new System.NotImplementedException();
    }
}
