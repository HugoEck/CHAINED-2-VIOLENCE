using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeToChain : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        ChargerManager charger = agent as ChargerManager;

        SetAnimation(agent);

        charger.chargeRunActive = true;

        agent.navigation.isStopped = true;
        Vector3 direction = (charger.chainPosition - charger.lastSavedPosition).normalized;
        agent.transform.position += direction * charger.chargingSpeed * Time.deltaTime;
        float distance = Vector3.Distance(agent.transform.position, charger.chainPosition);

        if (charger.chargeRunTimer < 0 || charger.collidedWithWall )
        {
            charger.chargeRunActive = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("Charger_Sprint", true);
        agent.animator.SetBool("Charger_Prepare", false);
        agent.animator.SetBool("Charger_Chase", false);
        agent.animator.SetBool("Charger_Attack", false);
        agent.animator.SetBool("Charger_Idle", false);
        agent.animator.SetBool("Charger_Electrocute", false);
        agent.animator.SetBool("Charger_Scared", false);
    }
}
