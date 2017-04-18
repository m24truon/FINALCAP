using UnityEngine;
using System.Collections;

public class ShopController : MonoBehaviour {

	public GameObject shopPanel;

	void OnTriggerEnter(Collider other)
	{
			OpenShop ();
	}

	void OpenShop()
	{
		shopPanel.SetActive (true);
		Time.timeScale = 0;
	}

	public void CloseShop()
	{
		shopPanel.SetActive (false);
		Time.timeScale = 1;
	}
}