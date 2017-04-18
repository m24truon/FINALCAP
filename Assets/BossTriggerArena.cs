using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerArena : MonoBehaviour {

	public GameObject Fences;

	void OnTriggerEnter (Collider other)
	{
		Fences.SetActive (true);
	}
		
	void BossDeath () {
		
		if (GameObject.FindGameObjectWithTag ("Boss") == null) 
		{
			Fences.SetActive (false);
		}

	}
	
	// Update is called once per frame
	void Update () {
		BossDeath ();
	}
}
