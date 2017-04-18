using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    private Collider[] colliders { get { return GetComponentsInChildren<Collider>(); }set { colliders = value; } }
    private Rigidbody[] rigidBodies { get { return GetComponentsInChildren<Rigidbody>(); } set { rigidBodies = value; } }
    private Animator animator { get { return GetComponentInParent<Animator>(); } set { animator = value; } }

    void Start()
    {
        if (colliders.Length == 0)
            return;
        if (rigidBodies.Length == 0)
            return;

        foreach (Collider col in colliders)
        {
            col.isTrigger = true;
        }

        foreach (Rigidbody r in rigidBodies)
        {
            r.isKinematic = true;
        }
    }

    public void Ragdoll()
    {
        if (animator == null)
            return;
        if (colliders.Length == 0)
            return;
        if (rigidBodies.Length == 0)
            return;

        animator.enabled = false;
        foreach(Collider col in colliders)
        {
            col.isTrigger = false;
        }

        foreach(Rigidbody r in rigidBodies)
        {
            r.isKinematic = false;
        }
    }
}
