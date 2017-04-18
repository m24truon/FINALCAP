using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static float health = 100;
    public GameObject player;
    public Slider healthBar;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("ReduceHealth", 1, 1);
    }

    void ReduceHealth()
    {
        health = health - 20;
        healthBar.value = health;
        if(health <= 0)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
