using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpgradeChoice : MonoBehaviour
{
	public TextMeshProUGUI text;
	public PlayerStats playerStats;
	public UpgradeableVariable currentVariable;
	public Button UpgradeButton;
    // Start is called before the first frame update
    void OnEnable()
    {
		currentVariable = playerStats.upgradeList.GetUpgradeableVariable();
		string modifier;
		float nextValue = currentVariable.initialValue + currentVariable.incrementBy;
		if(currentVariable.initialValue < nextValue)
		{
			modifier = "Increase";
		}
		else
		{
			modifier = "Decrease";
		}
		text.text = modifier + " " + currentVariable.name + " (" + string.Format("{0:0%}", currentVariable.initialValue) + " to " + string.Format("{0:0%}", nextValue) + ")";
    }

	public void Upgrade()
	{
		currentVariable.initialValue += currentVariable.incrementBy;
		switch(currentVariable.name)
		{
			case "HP per level":
				playerStats.hpMul += currentVariable.incrementBy;
				break;
			case "Damage":
				playerStats.dmgMul += currentVariable.incrementBy;
				break;
			case "Defence":
				playerStats.defMul += currentVariable.incrementBy;
				break;
			case "Movement Speed":
				playerStats.spdMul += currentVariable.incrementBy;
				break;
			case "Projectile Speed":
				playerStats.pSpdMul += currentVariable.incrementBy;
				break;
			case "Total Cooldown":
				playerStats.cdMul += currentVariable.incrementBy;
				if(currentVariable.initialValue <= currentVariable.maxValue)
				{
					playerStats.upgradeList.Remove(currentVariable);
				}
				break;
		}
		if(currentVariable.initialValue >= currentVariable.maxValue && currentVariable.name != "Total Cooldown")
		{
			playerStats.upgradeList.Remove(currentVariable);
		}
		playerStats.upgradeChoices.SetActive(false);
		
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		playerStats.upgradeList.resetChosen();
	}

}
