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

    //---------------RAGDOLL---------------------------
    AIPath aiPath;
    AIDestinationSetter destinationSetter;
    ObiCollider obiCollider;
    ObiRigidbody obiRb;
    Rigidbody[] rigidbodies;
    SimpleSmoothModifier smoothing;
    Collider[] capsuleColliders;

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

    public void BannerManBuff(float multiplier)
    {
        agent.currentHealth *= multiplier;
        agent.attack *= multiplier;
        agent.navigation.maxSpeed *= multiplier;
        agent.defense *= multiplier;
        agent.transform.localScale *= multiplier;

    }

    public void ToggleRagdoll(bool enabled)
    {
        if (!enabled)
        {
            agent.animator.enabled = true;

            foreach (Rigidbody rbs in rigidbodies)
            {
                rbs.isKinematic = true;
                rbs.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }

            foreach (Collider capsule1 in capsuleColliders)
            {
                capsule1.enabled = false;
            }
            agent.c_collider.enabled = true;
            agent.rb.isKinematic = false;
            obiCollider.enabled = true;
            obiRb.enabled = true;
            aiPath.enabled = true;
            smoothing.enabled = true;
            destinationSetter.enabled = true;
        }
        else
        {
            agent.animator.enabled = false;

            foreach (Rigidbody rbs in rigidbodies)
            {
                rbs.isKinematic = false;
                rbs.constraints = RigidbodyConstraints.None;
            }
            foreach (Collider capsule1 in capsuleColliders)
            {
                capsule1.enabled = true;
            }
            obiCollider.enabled = false;
            obiRb.enabled = false;
            aiPath.enabled = false;
            smoothing.enabled = false;
            destinationSetter.enabled = false;


        }
    }

    public void ToggleRagdoll(bool enabled, Transform affectingTransform)
    {
        if (!enabled)
        {
            agent.animator.enabled = true;

            foreach (Rigidbody rbs in rigidbodies)
            {
                rbs.isKinematic = true;
                rbs.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }

            foreach (Collider capsule1 in capsuleColliders)
            {
                capsule1.enabled = false;
            }
            agent.c_collider.enabled = true;
            agent.rb.isKinematic = false;
            obiCollider.enabled = true;
            obiRb.enabled = true;
            aiPath.enabled = true;
            smoothing.enabled = true;
            destinationSetter.enabled = true;
        }
        else
        {
            agent.animator.enabled = false;

            foreach (Rigidbody rbs in rigidbodies)
            {
                rbs.isKinematic = false;
                rbs.constraints = RigidbodyConstraints.None;
            }
            foreach (Collider capsule1 in capsuleColliders)
            {
                capsule1.enabled = true;
            }
            obiCollider.enabled = false;
            obiRb.enabled = false;
            aiPath.enabled = false;
            smoothing.enabled = false;
            destinationSetter.enabled = false;


        }
    }
    public void GetRagdollComponents(BaseManager agent)
    {
        aiPath = agent.GetComponent<AIPath>();
        destinationSetter = agent.GetComponent<AIDestinationSetter>();
        obiCollider = agent.GetComponent<ObiCollider>();
        capsuleColliders = agent.GetComponentsInChildren<Collider>();
        obiRb = agent.GetComponent<ObiRigidbody>();
        rigidbodies = agent.GetComponentsInChildren<Rigidbody>();
        smoothing = agent.GetComponent<SimpleSmoothModifier>();
    }
}
