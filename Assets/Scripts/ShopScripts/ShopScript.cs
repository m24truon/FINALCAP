using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopScript : MonoBehaviour {

	public WeaponHandler Guns;

	public void OnClickShotGun()
	{

		if (ScoreManager.score >= 100)
		{
			Guns.weaponsList.Add(Guns.ShotGun);
			ScoreManager.score -= 100;
		} 

		if (ScoreManager.score < 10) 
		{
			Debug.Log ("You don't have enough cash");
		}
	}

	public void OnClickSMG ()
	{
		if (ScoreManager.score >= 200)
		{
			Guns.weaponsList.Add(Guns.SMG);
			ScoreManager.score -= 200;

		} 

		if (ScoreManager.score < 10) 
		{
			Debug.Log ("You don't have enough cash");
		}
	}

	public void OnClickFuturisticGun()
	{
		if (ScoreManager.score >= 1000)
		{
			Guns.weaponsList.Add(Guns.SciFiGun);
			ScoreManager.score -= 1000;

		} 

		if (ScoreManager.score < 20) 
		{
			Debug.Log ("You don't have enough cash");
		}
	}

	public void OnClickBazzoka()
	{
		if (ScoreManager.score >= 30)
		{
			Guns.weaponsList.Add(Guns.Bazzoka);
			ScoreManager.score -= 30;

		} 

		if (ScoreManager.score < 30) 
		{
			Debug.Log ("You don't have enough cash");
		}
	}
}