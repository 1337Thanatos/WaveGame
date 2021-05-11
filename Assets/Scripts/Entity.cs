using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType
    {
        Structure,      // buildings or maybe stationary entities?
        GroundEntity,   // entities that move along the ground
        FlyingEntity    // entities that ignore normal pathing and fly
    }

    public enum AISeekType
    {
        Sticky,          //AI will stick to its current target, only changing targets if an external script does so.
        FirstFound,      //Whichever target is found first is stuck with, and doesn't change if target moves away or is out of line of sight
        LOS,             //if line of sight is broken, immediately seek new target or resume "normal" pathing (patrolling or heading to destination)
        Nearest,         //if another entity comes closer than current target switch to that (example, archer tries to take down melee who is closing range)
        Threat           //NYI but uses "threat" stat (also NYI) which makes entity prioritize targets.
    }

    [System.Serializable]
    public struct EntityStats
    {
        [Tooltip("Unique Entity ID (Not currently enforced)")]
        public readonly int EntityID;
        [Tooltip("Who owns this entity?")]
        public GameManager.ObjectOwner objectOwner;
        [Tooltip("Entity's type")]
        public EntityType entityType;
        [Tooltip("AI seek type, see code comments for explanation")]
        public AISeekType aISeekType;
        [Tooltip("ground or flight movement speed")]
        public float movementSpeed;
        [Tooltip("How high from the ground should the entity stay or try to stay?(if flying)")]
        public float flightHeight;

        [Tooltip("What is the maximum hitpoint value for this entity?")]
        public int MaxHP;
        [Tooltip("How many hitpoints the entity has")]
        public int HitPoints;
        [Tooltip("How much damage a basic attack by the entity does")]
        public int AttackDamage;
        [Tooltip("From which distance can this entity see other entities?")]
        public float DetectionRange;
        [Tooltip("What angle in front of the entity can be viewed?")]
        public float ViewAngle;

        [Tooltip("Is this a ranged or melee entity?")]
        public bool isRanged;
        [Tooltip("What is the attack range of the entity?")]
        public float AttackRange;
        [Tooltip("At runtime used to track the current waypoint the entity will try to reach")]
        public Transform CurrentWaypoint;
        [Tooltip("What layer of objects is this entity looking for?")]
        public LayerMask targetMask;

        [Header("TestFlags")]
        [Tooltip("Ignore line of sight for target detection?")]
        public bool ignoreLOS;
    }

    public EntityStats entityStats;
    public EntityAI AI;

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //FixedUpdate is used for physics queries
    void FixedUpdate()
    {

    }

    //returns true if a new target is needed and false if this entity is still alive.
    public bool TakeDamage(int damage)
    {
        

        if(this.entityStats.HitPoints>damage)
        {
            this.entityStats.HitPoints -= damage;
            return false;
        }
        else
        {
            Die(); //play death animation and after a few seconds destroy this entity
            return true;
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
        /* overridden for now
        AI.curAIState = EntityAI.AIState.Dying;
        AI.animator.Play(AI.DeathAnim);
        */
    }    
}
