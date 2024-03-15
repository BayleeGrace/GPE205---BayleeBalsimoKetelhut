using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerAIController : AIController
{

    #region Variables;
    private Health health; // Tracks this AI's health to trigger the enraged state
    public float healthToEnrage;
    public float enragedMoveSpeed; // Move speed multiplier when enraged
    public float enragedFireRate; // Fire rate multiplier when enraged
    #endregion Variables;
    
    public override void Start()
    {
        health = pawn.GetComponent<Health>();
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ProcessInputs()
    {
        base.ProcessInputs();
        switch(currentState) // Switch changes states
        {
                /*case AIState.Attack:
                base.DoAttackState();
                currentState = AIState.Attack;
                if (health.currentHealth <= healthToEnrage)
                {
                    ChangeState(AIState.Enraged);
                }
                if(!IsCanHear(target) || !IsDistanceLessThan(target, 12))
                {
                    ChangeState(AIState.Patrol);
                }
                break;*/

            case AIState.Enraged:
                DoEnragedState();
                currentState = AIState.Enraged;
                if(health.currentHealth > healthToEnrage)
                {
                    ChangeState(AIState.Attack);
                }
                break;
        }

    }

    public override void DoEnragedState()
    {
        if (target != null)
        {
            Enrage();
            // Chase
            Seek(target);
            // Shoot
            pawn.Shoot();
        }
        else if (target = null)
        {
            ChangeState(AIState.Patrol);
        }
    }

    public void Enrage()
    {
        pawn.moveSpeed = enragedMoveSpeed;
        pawn.fireRate = enragedFireRate;
    }

}
