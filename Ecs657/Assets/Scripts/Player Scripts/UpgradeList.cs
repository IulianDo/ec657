using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeList
{
	public static List<UpgradeableVariable> upgradeableList;
	public static int totalWeight;

	public UpgradeList(List<UpgradeableVariable> list)
	{
		upgradeableList = list;
		CalculateWeight();
	}

	public void Add(UpgradeableVariable variable)
	{
		upgradeableList.Add(variable);
		totalWeight += variable.weight;
		variable.actualWeight = totalWeight;
	}

	public void Remove(UpgradeableVariable variable)
	{
		for(int i = 0; i < upgradeableList.Count; i++)
		{
			if(variable.name == upgradeableList[i].name)
			{
				upgradeableList.RemoveAt(i);
				break;
			}
		}
		CalculateWeight();
	}

	public void CalculateWeight()
	{
		totalWeight = 0;
		foreach (UpgradeableVariable currentVariable in upgradeableList)
		{
			if (currentVariable.chosen == true)
			{
				continue;
			}
			totalWeight += currentVariable.weight;
			currentVariable.actualWeight = totalWeight;
		}
	}

	public UpgradeableVariable GetUpgradeableVariable()
	{
		UpgradeableVariable value = upgradeableList[0];
		int randValue = Random.Range(0, totalWeight);
		foreach (UpgradeableVariable currentVariable in upgradeableList)
		{
			if (randValue > currentVariable.actualWeight || currentVariable.chosen == true)
			{
				continue;
			}
			currentVariable.chosen = true;
			value = currentVariable;
			CalculateWeight();
			return value;
		}
		return value;
	}

	public void resetChosen()
	{
		foreach(UpgradeableVariable currentVariable in upgradeableList)
		{
			currentVariable.chosen = false;
		}
	}

	public void printVariable(UpgradeableVariable currentVariable)
	{
		Debug.Log(currentVariable.name + " " + currentVariable.chosen + " " + currentVariable.actualWeight);
	}

	public void printVariables()
	{
		foreach(UpgradeableVariable currentVariable in upgradeableList)
		{
			printVariable(currentVariable);
		}
	}
}
