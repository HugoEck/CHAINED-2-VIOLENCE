using Obi;
using Pathfinding;
using UnityEngine;

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
    BoxCollider box;
    CapsuleCollider capsule;

    enum GameMode
    {
        OnePlayer,
        TwoPlayer
    }
    GameMode gameMode = new GameMode();
    float distanceToPlayer1Sqr;
    float distanceToPlayer2Sqr;
    bool testBool;


    public AIBehaviorMethods(BaseManager manager)
    {
        agent = manager;

        //if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned == false)
        //{
        //    gameMode = GameMode.OnePlayer;
        //}
        //else
        //{
        //    gameMode = GameMode.TwoPlayer;
        //}

    }

    public Transform CalculateClosestTarget()
    {
        testBool = Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned == false)
        {
            gameMode = GameMode.OnePlayer;
        }
        else
        {
            gameMode = GameMode.TwoPlayer;
        }

        switch (gameMode)
        {
            case GameMode.OnePlayer:

                return agent.player1.transform;

            case GameMode.TwoPlayer:

                if (agent.playerManager1._bIsPlayerDisabled)
                {
                    return agent.player2.transform;
                }
                else if (agent.playerManager2._bIsPlayerDisabled)
                {
                    return agent.player1.transform;
                }
                distanceToPlayer1Sqr = (agent.transform.position - agent.player1.transform.position).sqrMagnitude;
                distanceToPlayer2Sqr = (agent.transform.position - agent.player2.transform.position).sqrMagnitude;

                if (distanceToPlayer1Sqr < distanceToPlayer2Sqr)
                {
                    return agent.player1.transform;
                }
                else
                {
                    return agent.player2.transform;
                }

            default:

                Debug.Log("AGENT CANNOT FIND PLAYER TARGET");
                return null;

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
            box.enabled = true;
            capsule.enabled = true;
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
            box.enabled = false;
            obiCollider.enabled = false;
            obiRb.enabled = false;
            aiPath.enabled = false;
            smoothing.enabled = false;
            destinationSetter.enabled = false;


        }
    }

    //public void ToggleRagdoll(bool enabled, Transform affectingTransform)
    //{
    //    if (!enabled)
    //    {
    //        agent.animator.enabled = true;

    //        foreach (Rigidbody rbs in rigidbodies)
    //        {
    //            rbs.isKinematic = true;
    //            rbs.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    //        }

    //        foreach (Collider capsule1 in capsuleColliders)
    //        {
    //            capsule1.enabled = false;
    //        }
    //        agent.c_collider.enabled = true;
    //        agent.rb.isKinematic = false;
    //        obiCollider.enabled = true;
    //        obiRb.enabled = true;
    //        aiPath.enabled = true;
    //        smoothing.enabled = true;
    //        destinationSetter.enabled = true;
    //    }
    //    else
    //    {
    //        agent.animator.enabled = false;

    //        foreach (Rigidbody rbs in rigidbodies)
    //        {
    //            rbs.isKinematic = false;
    //            rbs.constraints = RigidbodyConstraints.None;
    //        }
    //        foreach (Collider capsule1 in capsuleColliders)
    //        {
    //            capsule1.enabled = true;
    //        }
    //        obiCollider.enabled = false;
    //        obiRb.enabled = false;
    //        aiPath.enabled = false;
    //        smoothing.enabled = false;
    //        destinationSetter.enabled = false;


    //    }
    //}
    public void GetRagdollComponents(BaseManager agent)
    {
        box = agent.GetComponent<BoxCollider>();
        capsule = agent.GetComponent<CapsuleCollider>();
        aiPath = agent.GetComponent<AIPath>();
        destinationSetter = agent.GetComponent<AIDestinationSetter>();
        obiCollider = agent.GetComponent<ObiCollider>();
        capsuleColliders = agent.GetComponentsInChildren<Collider>();
        obiRb = agent.GetComponent<ObiRigidbody>();
        rigidbodies = agent.GetComponentsInChildren<Rigidbody>();
        smoothing = agent.GetComponent<SimpleSmoothModifier>();
    }
}
