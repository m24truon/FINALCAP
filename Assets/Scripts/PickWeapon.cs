using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickWeapon : MonoBehaviour
{
    Weapon weapon;
    WeaponHandler weaponHandler;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            weaponHandler.weaponsList.Add(weaponHandler.currentWeapon);
            Debug.Log("hello");
        }
    }

   
}
