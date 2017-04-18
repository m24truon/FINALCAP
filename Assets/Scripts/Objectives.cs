using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour {

	public GameObject Checkpoint;
	public GameObject RemoveCastle;

	void OnTriggerEnter(Collider other)
	{
		EndGoalSet();
		CastleRemove ();
		Destroy (gameObject);
	}

	// Use this for initialization
	void EndGoalSet () {
		Checkpoint.SetActive (true);
	}
	
	// Update is called once per frame
	void CastleRemove () {
		RemoveCastle.SetActive (false);
	}
}
