using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStats : MonoBehaviour
{

    [SerializeField] private Menus menus;
    //_______________________________________________________//
    #region hpVariables
    [Header("Health Settings")]
    [SerializeField] private int maxHitPoints;
    [SerializeField] private int hitPoints;
    #endregion
    //_______________________________________________________//
    #region playerMultipliers
    [Header("Player Multipliers")]
    public float hpMul = 1.1f;
    public float dmgMul = 1;
    public float defMul = 1;
    public float spdMul = 1;
    public float pSpdMul = 1;
    public float cdMul = 1;
    public UpgradeList upgradeList;
    public GameObject upgradeChoices;
	#endregion
	//_______________________________________________________//
	#region XP variables
	// XP variables
	[Header("XP Variables")]
    [SerializeField] private float experienceTillNextLevel;
    [SerializeField] private float experienceNeededMultiplier;
    [SerializeField] private float currentExperience;
    [SerializeField] private int level;
	#endregion
	//_______________________________________________________//
	#region UI variables
	// UI variables
	[Header("UI Variables")]
    [SerializeField] private TMP_Text LevelUI;
    [SerializeField] private HealthBar healthbar;
	#endregion
	//_______________________________________________________//
	// Start is called before the first frame update
	void Start()
    {
        hitPoints = maxHitPoints;
        level = 0;
        healthbar.setMaxHealth(maxHitPoints);
        LevelUI.text = "Level " + level + ": (" + Mathf.Round(currentExperience) + "/" + Mathf.Round(experienceTillNextLevel) + ")";

        //List of Upgrade Variables with weighted values for getting a random one
        List<UpgradeableVariable> list = new List<UpgradeableVariable>();
        list.Add(new UpgradeableVariable("HP per level", ref hpMul, 2, 0.1f, 10));
        list.Add(new UpgradeableVariable("Damage", ref dmgMul, 2, 0.1f, 10));
        list.Add(new UpgradeableVariable("Defence", ref defMul, 2, 0.1f, 10));
        list.Add(new UpgradeableVariable("Movement Speed", ref spdMul, 2, 0.1f, 10));
        list.Add(new UpgradeableVariable("Projectile Speed", ref spdMul, 2, 0.1f, 10));
        list.Add(new UpgradeableVariable("Total Cooldown", ref cdMul, 0.1f, -0.1f, 100));
        upgradeList = new UpgradeList(list);

        print(upgradeList);
    }
    //------------------------------------------------------------------//
	#region hpCode
	// Allows the player to take damage as an int
	public void TakeDamage(int amount)
    {
        hitPoints -= Mathf.RoundToInt((float)amount*defMul);
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

    public bool AtMaxHP()
    {
        return maxHitPoints == hitPoints;
    }

    #endregion;
    //------------------------------------------------------------------//
    #region xpCode
    public void AddXp(float value)
	{
        currentExperience += value;
        while(currentExperience >= experienceTillNextLevel)
		{
            maxHitPoints = (int) (maxHitPoints * hpMul);
            //increase hp by x amount and full heal
            healthbar.setMaxHealth(maxHitPoints);
            Heal(maxHitPoints);
            upgradeChoices.SetActive(true);

            currentExperience -= experienceTillNextLevel;
            experienceTillNextLevel *= experienceNeededMultiplier;
            level++;
		}
        LevelUI.text = "Level " + level + ": (" + Mathf.Round(currentExperience) + "/" + Mathf.Round(experienceTillNextLevel) + ")"; 
	}

    private void ShowChoices()
	{

	}

    public void upgradeVariable(float Value)
	{

	}
	#endregion
	//------------------------------------------------------------------//
}
