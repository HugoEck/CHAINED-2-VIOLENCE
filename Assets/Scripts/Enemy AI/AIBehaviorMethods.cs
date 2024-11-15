using Obi;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AIBehaviorMethods
{
    BaseManager agent;
    float lastAttackedTime;

    public AIBehaviorMethods(BaseManager manager)
    {
        agent = manager;
    }

    public Transform CalculateClosestTarget()
    {
        float distanceToPlayer1Sqr = (agent.transform.position - agent.player1.transform.position).sqrMagnitude;
        float distanceToPlayer2Sqr = (agent.transform.position - agent.player2.transform.position).sqrMagnitude;

        if (distanceToPlayer1Sqr < distanceToPlayer2Sqr)
        {
            return agent.player1.transform;
        }
        else
        {
            return agent.player2.transform;
        }
    }
    //public Transform CalculateClosestTarget()
    //{
    //    if (Vector3.Distance(agent.transform.position, agent.player1.transform.position) < Vector3.Distance(agent.transform.position, agent.player2.transform.position))
    //    {
    //        return agent.player1.transform;
    //    }
    //    else if ((Vector3.Distance(agent.transform.position, agent.player2.transform.position) < Vector3.Distance(agent.transform.position, agent.player1.transform.position)))
    //    {
    //        return agent.player2.transform;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    public bool IsAttackAllowed()
    {

        if (Time.time > lastAttackedTime + agent.attackSpeed)
        {
            lastAttackedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    public Player GetCorrectPlayerManager(Transform player)
    {
        if (player == agent.player1.transform)
        {
            return agent.playerManager1;
        }
        else
        {
            return agent.playerManager2;
        }
    }

    public Vector3 CalculateChainPosition()
    {

        Vector3 p1Position = agent.player1.transform.position;
        Vector3 p2Position = agent.player2.transform.position;
        Vector3 midPoint = (p1Position + p2Position) / 2;
        midPoint.y = 0;
        return midPoint;
    }

    public void RotateTowardsClosestPlayer()
    {
        agent.targetedPlayer = CalculateClosestTarget();
        Vector3 direction = (agent.targetedPlayer.position - agent.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }

    public void ToggleRagdoll(bool enabled)
    {
        if (!enabled)
        {
            Rigidbody[] rigidbodies = agent.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rbs in rigidbodies)
            {
                rbs.isKinematic = true; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
                rbs.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }
            Collider[] capsuleColliders = agent.GetComponentsInChildren<Collider>();
            foreach (Collider capsule1 in capsuleColliders)
            {
                capsule1.enabled = false; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
            }
            agent.c_collider.enabled = true;
            agent.rb.isKinematic = false;
        }
        else
        {
            AIPath aiPath = agent.GetComponent<AIPath>();
            AIDestinationSetter destinationSetter = agent.GetComponent<AIDestinationSetter>();
            ObiCollider obiCollider = agent.GetComponent<ObiCollider>();
            ObiRigidbody obiRb = agent.GetComponent<ObiRigidbody>();
            Rigidbody[] rigidbodies = agent.GetComponentsInChildren<Rigidbody>();
            SimpleSmoothModifier smoothing = agent.GetComponent<SimpleSmoothModifier>();
            foreach (Rigidbody rbs in rigidbodies)
            {
                rbs.isKinematic = false; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
                rbs.constraints = RigidbodyConstraints.None;
                Collider[] capsuleColliders = agent.GetComponentsInChildren<Collider>();
                foreach (Collider capsule1 in capsuleColliders)
                {
                    capsule1.enabled = true; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
                }
            }
            //c_collider.enabled = false;
            //rb.isKinematic = true;
            obiCollider.enabled = false;
            obiRb.enabled = false;
            aiPath.enabled = false;
            smoothing.enabled = false;
            destinationSetter.enabled = false;
        }
    }
}
