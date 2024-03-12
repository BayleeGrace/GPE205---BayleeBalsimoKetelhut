using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerAIController : AIController
{
    private Health health;
    public float healthToEnrage;
    public float enragedMoveSpeed;
    public float enragedFireRate;
    
    public override void Start()
    {
        health = pawn.GetComponent<Health>();
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ProcessInputs()
    {
        base.ProcessInputs();
        switch(currentState) // Switch changes states
        {
                
                case AIState.Attack:
                DoAttackState();
                currentState = AIState.Attack;
                if (health.currentHealth <= healthToEnrage)
                {
                    ChangeState(AIState.Enraged);
                }
                if(!IsCanHear(target) || !IsDistanceLessThan(target, 12))
                {
                    ChangeState(AIState.Patrol);
                }
                break;

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
            // Chase
            Seek(target);
            // Shoot
            pawn.Shoot();
            Enrage();
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