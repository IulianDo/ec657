using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private Menus menus;
    #region hpVariables
    [SerializeField] private  int maxHitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private  int hpIncreasePerLevel;
    #endregion

    #region playerMultipliers
    public float dmgMul=1;
    #endregion

    //_______________________________________________________//
    // XP variables
    [SerializeField] private float experienceTillNextLevel;
    [SerializeField] private float experienceNeededMultiplier;
    [SerializeField] private float currentExperience;
    [SerializeField] private int level;
    //_______________________________________________________//
    // UI variables
    [SerializeField] private TMP_Text LevelUI;
    [SerializeField] private HealthBar healthbar;
    //_______________________________________________________//
    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHitPoints;
        level = 0;
        healthbar.setMaxHealth(maxHitPoints);
        LevelUI.text = "Level " + level + ": (" + Mathf.Round(currentExperience) + "/" + Mathf.Round(experienceTillNextLevel) + ")";
    }
    //------------------------------------------------------------------//
	#region hpCode
	// Update is called once per frame
	// Allows the player to take damage as an int
	public void TakeDamage(int amount)
    {
        hitPoints -= amount;
        healthbar.setHealth(hitPoints);
        if (hitPoints <= 0)
        {
            menus.GameOver();
        }   
    }

    // Increase hp by x amount 
    public void Heal(int amount)
    {
        hitPoints += amount;
        //if healed over max hp, clamp to max hp
        if (hitPoints >= maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }
        healthbar.setHealth(hitPoints);
    }
    #endregion;
    //------------------------------------------------------------------//
    #region xpCode
    public void AddXp(float value)
	{
        currentExperience += value;
        while(currentExperience >= experienceTillNextLevel)
		{
            maxHitPoints += hpIncreasePerLevel;
            //increase hp by x amount and full heal
            healthbar.setMaxHealth(maxHitPoints);
            Heal(maxHitPoints);
            dmgMul += 0.1f;

            currentExperience -= experienceTillNextLevel;
            experienceTillNextLevel *= experienceNeededMultiplier;
            level++;
		}
        LevelUI.text = "Level " + level + ": (" + Mathf.Round(currentExperience) + "/" + Mathf.Round(experienceTillNextLevel) + ")"; 
	}

	#endregion
	//------------------------------------------------------------------//
}
