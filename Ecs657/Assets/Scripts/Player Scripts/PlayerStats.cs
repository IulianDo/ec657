using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerStats : MonoBehaviour
{

    [SerializeField] private Menus menus;
    //_______________________________________________________//
    #region hpVariables
    [Header("Health Settings")]
    [SerializeField] private int maxHitPoints;
    [SerializeField] private int hitPoints;
    [SerializeField] private int baseHitPoints;
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
    public float valueChange = 0.1f;
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
    private int levelsToAdd = 0;
	#endregion
	//_______________________________________________________//
	#region UI variables
	// UI variables
	[Header("UI Variables")]
    [SerializeField] private TMP_Text LevelUI;
    [SerializeField] private HealthBar healthbar;
    [SerializeField] private Sprite health;
    [SerializeField] private Sprite damage;
    [SerializeField] private Sprite defence;
    [SerializeField] private Sprite speed;
    [SerializeField] private Sprite projectileSpeed;
    [SerializeField] private Sprite cooldown;
	#endregion
	//_______________________________________________________//
	// Start is called before the first frame update

	void Start()
	{
		DontDestroyOnLoad(this);

        hitPoints = maxHitPoints;
        level = 0;
        healthbar.setMaxHealth(maxHitPoints);
        LevelUI.text = "Level " + level + ": (" + Mathf.Round(currentExperience) + "/" + Mathf.Round(experienceTillNextLevel) + ")";

        //set difficulty, default to easy if it cannot find one
        GameObject DifficultyManager = GameObject.Find("DifficultyManager");
        if(DifficultyManager != null)
		{
			string difficulty = DifficultyManager.GetComponent<DifficultyManager>().difficulty;
            SetDifficulty(difficulty);
        }
        else
		{
            SetDifficulty("Medium");
        }
	}

    void Update()
    {
        if(levelsToAdd > 0 && Time.timeScale != 0)
        {
            ShowChoices();
        }
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
            maxHitPoints += (int) (baseHitPoints * hpMul);
            //increase hp by x amount and full heal
            healthbar.setMaxHealth(maxHitPoints);
            Heal(maxHitPoints);
            currentExperience -= experienceTillNextLevel;
            experienceTillNextLevel *= experienceNeededMultiplier;
            level++;
            levelsToAdd ++;
		}
        LevelUI.text = "Level " + level + ": (" + Mathf.Round(currentExperience) + "/" + Mathf.Round(experienceTillNextLevel) + ")"; 
	}

    private void ShowChoices()
	{
        Time.timeScale = 0f;
        levelsToAdd --;
        upgradeChoices.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
	}

    public void upgradeVariable(float Value)
	{

	}
    #endregion
    //------------------------------------------------------------------//
    #region DifficultySetting Code
    public void SetDifficulty(string difficulty)
	{
        switch(difficulty)
		{
            case "Easy":
                valueChange = 0.2f;
                hpMul = 1.2f;
                break;
            case "Medium":
                valueChange = 0.1f;
                break;
            case "Hard":
                valueChange = 0.05f;
                break;
		}

        //List of Upgrade Variables with weighted values for getting a random one
        List<UpgradeableVariable> list = new List<UpgradeableVariable>();
        list.Add(new UpgradeableVariable("HP per level", ref hpMul, 3f, valueChange, 5, health));
        list.Add(new UpgradeableVariable("Damage", ref dmgMul, 4f, valueChange, 10, damage));
        list.Add(new UpgradeableVariable("Defence", ref defMul, 4f, valueChange, 10, defence));
        list.Add(new UpgradeableVariable("Movement Speed", ref spdMul, 4f, valueChange, 5, speed));
        list.Add(new UpgradeableVariable("Projectile Speed", ref spdMul, 4f, valueChange, 20, projectileSpeed));
        list.Add(new UpgradeableVariable("Total Cooldown", ref cdMul,   0.1f, -valueChange, 5, cooldown));
        upgradeList = new UpgradeList(list);
    }
    #endregion
    //------------------------------------------------------------------//
}
