using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityAI : MonoBehaviour
{
    public enum AIState
    {
        Passive,        //Idle state
        Follow,
        Waypoints,
        Aggressive,
        Dying           //Does nothing, is handled by Entity Die function.
    }

    [Tooltip("Current AI State, also used to store initial AI State. If left default the AI remains passive.")]
    public AIState curAIState = AIState.Passive;

    [Tooltip("The parent entity, used mostly to store stats")]
    public Entity parentEntity;

    [Tooltip("The animator component of this entity")]
    public Animator animator;

    [Tooltip("The navmeshagent of this entity")]
    public NavMeshAgent meshAgent;

    [Tooltip("The name of the animation to play when the entity is idling")]
    public string PassiveStateAnim;
    [Tooltip("The name of the animation to play when the entity is walking")]
    public string WalkingStateAnim;
    [Tooltip("The name of the animation to play when the entity is using a basic attack")]
    public string AttackAnim;
    [Tooltip("The name of the animation to play when the entity is dying")]
    public string DeathAnim;

    [Tooltip("Current target of the entity")]
    public GameObject CurrentTarget;

    [Tooltip("Holds current destination of this entity")]
    public Vector3 targetPosition;

    //used to keep track of multiple targets
    [System.Serializable]
    public struct ThreatList
    {
        public float Threat;
        public GameObject ThreatSource;
    }

    public ThreatList[] threatlist;

    // Start is called before the first frame update
    void Start()
    {
        //try to Fetch necessary components at runtime unless already set. If they aren't set and can't be found then send error message.
        if (parentEntity == null)
        {
            parentEntity = this.gameObject.GetComponent<Entity>();
            if(parentEntity == null)
            {
                Debug.LogError("NO Entity COMPONENT FOUND ON GAMEOBJECT WITH AN ENTITYAI ATTACHED!");
            }
        }

        if(animator ==  null)
        {
            animator = this.gameObject.GetComponent<Animator>();
            if(animator == null)
            {
                Debug.LogError("NO Animator COMPONENT FOUND ON GAMEOBJECT WITH AN ENTITYAI ATTACHED!");
            }
        }

        if (meshAgent == null)
        {
            meshAgent = this.gameObject.GetComponent<NavMeshAgent>();
            if (meshAgent == null)
            {
                Debug.LogError("NO NavMeshAgent COMPONENT FOUND ON GAMEOBJECT WITH AN ENTITYAI ATTACHED!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //What is the AI's current state? then execute appropriate actions.
        switch(curAIState)
        {
            //If the AI's state is currently passive, then start or continue playing the passive animation.
            case AIState.Passive:
                animator.Play(PassiveStateAnim);
                break;

            case AIState.Follow:
                //has the target moved since the last frame?
                if (CurrentTarget != null)
                {
                    targetPosition = CurrentTarget.transform.position;

                    if (meshAgent.destination != targetPosition)
                    {
                        meshAgent.SetDestination(targetPosition);

                        animator.SetBool("Walk Forward", true);
                    }  
                }
                else
                {
                    FindTarget();
                }
                break;

            case AIState.Waypoints:

                break;

            case AIState.Aggressive:
                //has the target moved since the last frame?
                if (CurrentTarget != null)
                {
                    targetPosition = CurrentTarget.transform.position;

                    if ( Vector3.Distance(this.gameObject.transform.position , targetPosition) > parentEntity.entityStats.AttackRange)
                    {
                        meshAgent.isStopped = false;
                        meshAgent.SetDestination(targetPosition);

                        animator.SetBool("Walk Forward", true);
                    }
                    else
                    {
                        meshAgent.isStopped = true;
                        animator.Play(AttackAnim);                        
                    }
                }
                else
                {
                    FindTarget();
                }
                break;
        }
    }

    public void DealDamage()
    {
        CurrentTarget.GetComponent<Entity>().TakeDamage(this.parentEntity.entityStats.AttackDamage);
    }

    public void FindTarget()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, parentEntity.entityStats.DetectionRange, parentEntity.entityStats.targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            if (!parentEntity.entityStats.ignoreLOS)
            {
                Transform target = targetsInViewRadius[i].transform;

                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < parentEntity.entityStats.ViewAngle / 2)
                {

                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, parentEntity.entityStats.targetMask))
                    {

                        if (CurrentTarget != null)
                        {
                            Vector3 dirToCurTarget = (CurrentTarget.transform.position - transform.position).normalized;

                            if (!Physics.Raycast(transform.position, dirToCurTarget, distanceToTarget, parentEntity.entityStats.targetMask))
                            {
                                float currentTargetDistance = Vector3.Distance(transform.position, CurrentTarget.transform.position);

                                if (distanceToTarget < currentTargetDistance)
                                {
                                    CurrentTarget = target.gameObject;
                                }
                            }
                            else
                            {
                                CurrentTarget = target.gameObject;
                            }
                        }
                        else
                        {
                            CurrentTarget = target.gameObject;
                        }
                    }
                    else
                    {
                        Debug.Log("Finding target...");
                    }
                }
            }
            else
            {
                //currently just grabs the first target it sees and sticks with it, improve if further testing without LOS is needed.
                if (CurrentTarget == null)
                {
                    CurrentTarget = targetsInViewRadius[i].gameObject;
                }
            }
        }
    }
}
