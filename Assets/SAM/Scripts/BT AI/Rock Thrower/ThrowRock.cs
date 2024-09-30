using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        RockThrowerManager rockThrower = agent as RockThrowerManager;

        if (rockThrower == null)
        {
            Debug.Log("Denna metod fungerar inte!");
        }

        if (agent.IsAttackAllowed())
        {
            GameObject rock = GameObject.Instantiate(rockThrower.rockPrefab, rockThrower.throwPoint.position, rockThrower.throwPoint.rotation);
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            RockDestroyer rd = rock.GetComponent<RockDestroyer>();

            rd.damage = agent.damage;
            rb.velocity = rockThrower.calculatedVelocity;
        }
        

        return NodeState.RUNNING;
    }
    
}
