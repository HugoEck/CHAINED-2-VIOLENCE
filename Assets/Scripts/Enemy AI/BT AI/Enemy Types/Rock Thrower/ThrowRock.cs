using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        agent.animator.SetBool("RockThrower_Chase", false);
        
        RockThrowerManager rockThrower = agent as RockThrowerManager;

        if (agent.IsAttackAllowed())
        {
            agent.animator.SetBool("RockThrower_Attack", true);
            agent.animator.SetBool("RockThrower_Idle", false);

            GameObject rock = GameObject.Instantiate(rockThrower.rockPrefab, rockThrower.throwPoint.position, rockThrower.throwPoint.rotation);
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            RockDestroyer rd = rock.GetComponent<RockDestroyer>();

            rd.damage = agent.attack;
            rb.velocity = rockThrower.calculatedVelocity;
        }
        else
        {
            agent.animator.SetBool("RockThrower_Idle", true);
            agent.animator.SetBool("RockThrower_Attack", false);
        }
        

        return NodeState.RUNNING;
    }
    
}
