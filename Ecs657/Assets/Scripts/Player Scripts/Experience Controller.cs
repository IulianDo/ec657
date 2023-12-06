using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    [SerializeField] private int velocityMultiplier;
    [SerializeField] private bool chasingPlayer;
    private GameObject player;
    [SerializeField] private GameObject self;
    [SerializeField] private float xp;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Allows for changes in the value of xp
    public void SetXp(float value)
	{
        xp = value;
	}

    //in late update to avoid choppiness in movementzs
    void LateUpdate()
    {
        //movement code for xpOrb to go to player
        if (chasingPlayer)
        {
            Transform character = player.transform;
            Vector3 direction = (character.position - transform.position).normalized;
            this.transform.Translate(direction * Time.deltaTime * velocityMultiplier);
        }
    }

    // Allows for xp to follow and be collected by the player
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().AddXp(xp);
            Destroy(self);
        }
        else if (other.CompareTag("xpPickUp"))
        {
            player = other.gameObject;
            chasingPlayer = true;
        }
    }
}