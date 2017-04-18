using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    Animator anim;
    private CharacterController characterController { get { return GetComponent<CharacterController>(); } set { characterController = value; } }
    private RagdollManager ragdollManager { get { return GetComponentInChildren<RagdollManager>(); } set { ragdollManager = value; } }

    public float health = 100;
    public int faction;
    private SoundController sc;
	public int scoreValue = 10;  
    public GameObject player;
    public Slider healthBar;

    public GameObject CBTprefab;

    public MonoBehaviour[] scriptsToDisable;


    public Transform soundPos;

    [System.Serializable]
    public class SoundSettings
    {
        public AudioClip attackSounds;
        public AudioClip reloadSound;
        [Range(0, 3)]
        public float pitchMin = 1;
        [Range(0, 3)]
        public float pitchMax = 1.2f;
        public AudioSource audioS;
    }
    [SerializeField]
    public SoundSettings sounds;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        healthFix();
        if (health <= 0)
        {
            Die();
            anim.SetBool("isDead", true);
            Destroy(this.gameObject, 3);

        }
        //health = Mathf.Clamp(health, 0, 100);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Skeleton")
        {
            Debug.Log("hit");
            health -= 10;
        }

        if (other.gameObject.tag == "HealthPack")
        {
            Debug.Log("healed.");
            health += 25;
        }

        if(other.gameObject.tag == "Zombie")
        {
            Debug.Log("Zombie");
            health -= 1;
        }

        if(other.gameObject.tag == "Boss")
        {
            health -= 20;
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        healthBar.value = health;
        InitCBT(damage.ToString());
        

    }
    void healthFix()
    {
        healthBar.value = health;
    }



    public void Die()
    {
        //characterController.enabled = false;
        if (scriptsToDisable.Length == 0)
        {
            Debug.Log("All scripts still running on this character but he is dead");

            return;
        }

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            script.enabled = false;
        }

        if (ragdollManager != null)
            ragdollManager.Ragdoll();

		if (health == 0) 
		{
			ScoreManager.score += scoreValue;
		}
    }

    void InitCBT(string text)
    {
        GameObject temp = Instantiate(CBTprefab) as GameObject;
        RectTransform tempRect = temp.GetComponent<RectTransform>();
        temp.transform.SetParent(transform.FindChild("EnemyCanvas"));
        tempRect.transform.localPosition = CBTprefab.transform.localPosition;
        tempRect.transform.localScale = CBTprefab.transform.localScale;
        tempRect.transform.localRotation = CBTprefab.transform.localRotation;

        temp.GetComponent<Text>().text = text;
        temp.GetComponent<Animator>().SetTrigger("Hit");
        Destroy(temp.gameObject, 2);
    }
}
