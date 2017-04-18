using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRigid : MonoBehaviour
{

    public float fpsTargetDistance;
    public float enemyLookDistance;
    public float attackDistance;
    public float enemyMovementSpeed;
    public float damping;
    public Transform fpsTarget;
    Rigidbody theRigidbody;
    Renderer myRenderer;
    Animator anim;


    string state = "patrol";
    public GameObject[] waypoints;
    int currentWP;
    float accuracyWP = 1.0f;

    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        theRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fpsTargetDistance = Vector3.Distance(fpsTarget.position, transform.position);
        if(fpsTargetDistance < enemyLookDistance)
        {
            myRenderer.material.color = Color.yellow;
            LookAtPlayer();
            print("Look at player");
        }
        if(fpsTargetDistance < attackDistance)
        {
            Attack();
            print("attack");
        }
        else
        {
            myRenderer.material.color = Color.blue;
        }
    }

    void LookAtPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(fpsTarget.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    void Attack()
    {
        theRigidbody.AddForce(transform.forward * enemyMovementSpeed);
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", true);
    }
}
