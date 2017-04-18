
using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Animator))]
public class AI : MonoBehaviour
{

    private UnityEngine.AI.NavMeshAgent navmesh;
    private CharacterMovement characterMove { get { return GetComponent<CharacterMovement>(); } set { characterMove = value; } }
    private Animator animator { get { return GetComponent<Animator>(); } set { animator = value; } }
    private CharacterStats characterStats { get { return GetComponent<CharacterStats>(); } set { characterStats = value; } }
    private WeaponHandler weaponHandler { get { return GetComponent<WeaponHandler>(); } set { weaponHandler = value; } }

    public enum AIState { Patrol, Attack, FindEnemy, FindCover }
    public AIState aiState;

    [System.Serializable]
    public class PatrolSettings
    {
        public WaypointBase[] waypoints;
    }
    public PatrolSettings patrolSettings;

    [System.Serializable]
    public class SightSettings
    {
        public LayerMask sightLayers;
        public float sightRange = 30f;
        public float fieldOfView = 120f;
        public float eyeheight;
    }
    public SightSettings sight;

    [System.Serializable]
    public class AttackSettings
    {
        public float fireChance = 0.1f;
    }
    public AttackSettings attack;

    private float currentWaitTime;
    private int waypointIndex;
    private Transform currentLookTransform;
    private bool walkingToDest;
    private bool setDestination;
    private bool reachedDestination;

    private float forward;

    private Transform target;
    private Vector3 targetLastKnownPosition;
    private CharacterStats[] allCharacters;

    private bool aiming;

    // Use this for initialization
    void Start()
    {
        navmesh = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();

        if (navmesh == null)
        {
            Debug.LogError("We need a navmesh to traverse the world with.");
            enabled = false;
            return;
        }

        if (navmesh.transform == this.transform)
        {
            Debug.LogError("The navmesh agent should be a child of the character: " + gameObject.name);
            enabled = false;
            return;
        }

        navmesh.speed = 0;
        navmesh.acceleration = 0;
        navmesh.autoBraking = false;

        if (navmesh.stoppingDistance == 0)
        {
            Debug.Log("Auto settings stopping distance to 1.3f");
            navmesh.stoppingDistance = 1.3f;
        }

        GetAllCharacters();
    }

    void GetAllCharacters()
    {
        allCharacters = GameObject.FindObjectsOfType<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {

        //TODO: Animate the strafe when the enemy is trying to shoot us.
        characterMove.Animate(forward, 0);
        navmesh.transform.position = transform.position;

        weaponHandler.Aim(aiming);

        LookForTarget();



        switch (aiState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Attack:
                FireAtEnemy();
                Patrol();
                break;


        }
    }

    void LookForTarget()
    {
        if (allCharacters.Length > 0)
        {
            foreach (CharacterStats c in allCharacters)
            {
                if (c != characterStats && c.faction != characterStats.faction && c == ClosestEnemy())
                {
                    RaycastHit hit;
                    Vector3 start = transform.position + (transform.up * sight.eyeheight);
                    Vector3 dir = (c.transform.position + c.transform.up) - start;
                    float sightAngle = Vector3.Angle(dir, transform.forward);
                    if (Physics.Raycast(start, dir, out hit, sight.sightRange, sight.sightLayers) &&
                        sightAngle < sight.fieldOfView && hit.collider.GetComponent<CharacterStats>())
                    {
                        target = hit.transform;
                        targetLastKnownPosition = Vector3.zero;
                    }
                    else
                    {
                        if (target != null)
                        {
                            targetLastKnownPosition = target.position;
                            target = null;
                        }
                    }
                }
            }
        }
    }

    CharacterStats ClosestEnemy()
    {
        CharacterStats closestCharacter = null;
        float minDistance = Mathf.Infinity;
        foreach (CharacterStats c in allCharacters)
        {
            if (c != characterStats && c.faction != characterStats.faction)
            {
                float distToCharacter = Vector3.Distance(c.transform.position, transform.position);
                if (distToCharacter < minDistance)
                {
                    closestCharacter = c;
                    minDistance = distToCharacter;
                }
            }
        }

        return closestCharacter;
    }

    void Patrol()
    {


        if (target == null)
        {
            PatrolBehaviour();
            if (!navmesh.isOnNavMesh)
            {
                Debug.Log("We're off the navmesh");
                return;
            }

            if (patrolSettings.waypoints.Length == 0)
                return;

            if (!setDestination)
            {
                navmesh.SetDestination(patrolSettings.waypoints[waypointIndex].destination.position);
                setDestination = true;
            }

            if ((navmesh.remainingDistance <= navmesh.stoppingDistance) || reachedDestination && !navmesh.pathPending)
            {
                setDestination = false;
                walkingToDest = false;
                forward = LerpSpeed(forward, 0, 15);
                currentWaitTime -= Time.deltaTime;

                if (patrolSettings.waypoints[waypointIndex].lookAtTarget != null)
                    currentLookTransform = patrolSettings.waypoints[waypointIndex].lookAtTarget;
                if (currentWaitTime <= 0)
                {
                    waypointIndex = (waypointIndex + 1) % patrolSettings.waypoints.Length;
                    reachedDestination = false;
                }
                else
                {
                    reachedDestination = true;
                }

            }
            else
            {
                LookAtPosition(navmesh.steeringTarget);
                walkingToDest = true;
                forward = LerpSpeed(forward, 0.5f, 15);
                currentWaitTime = patrolSettings.waypoints[waypointIndex].waitTime;
                currentLookTransform = null;
            }
        }
        else
        {
            aiState = AIState.Attack;
        }

    }

    void FireAtEnemy()
    {
        if (target != null && target.gameObject.tag == "Player")
        {
            AttackBehaviour();
            LookAtPosition(target.position);
            Vector3 start = transform.position + transform.up;
            Vector3 dir = target.position - transform.position;
            Ray ray = new Ray(start, dir);
            if (Random.value <= attack.fireChance)
                weaponHandler.FireCurrentWeapon(ray);
        }
    }

    float LerpSpeed(float curSpeed, float destSpeed, float time)
    {
        curSpeed = Mathf.Lerp(curSpeed, destSpeed, Time.deltaTime * time);
        return curSpeed;
    }

    void LookAtPosition(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.x = 0;
        lookRot.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * 5);
    }

    void OnAnimatorIK()
    {
        if (currentLookTransform != null && !walkingToDest)
        {
            animator.SetLookAtPosition(currentLookTransform.position);
            animator.SetLookAtWeight(1, 0, 0.5f, 0.7f);
        }
        else if (target != null)
        {
            float dist = Vector3.Distance(target.position, transform.position);
            if (dist > 3)
            {
                animator.SetLookAtPosition(target.transform.position + transform.right * 0.3f);
                animator.SetLookAtWeight(1, 1, 0.3f, 0.2f);
            }
            else
            {


                animator.SetLookAtPosition(target.transform.position + target.up + transform.right * 0.3f);
                animator.SetLookAtWeight(1, 1, 0.3f, 0.2f);
            }
        }

    }

    void PatrolBehaviour()
    {
        aiming = false;
    }

    void AttackBehaviour()
    {
        aiming = true;
        walkingToDest = false;
        setDestination = false;
        reachedDestination = false;
        forward = LerpSpeed(forward, 0, 15);
        currentLookTransform = null;
    }
}

[System.Serializable]
public class WaypointBase
{
    public Transform destination;
    public float waitTime;
    public Transform lookAtTarget;
}
