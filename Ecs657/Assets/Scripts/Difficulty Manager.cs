using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DifficultyManager: MonoBehaviour
{
	public string difficulty;
	public TextMeshProUGUI text;
	public static DifficultyManager instance;
	private int value;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		value = 1;
		DontDestroyOnLoad(this);
	}

	public void changeDifficulty()
	{
		value += 1;
		if(value > 2)
		{
			value = 0;
		}
		switch(value)
		{
			case 0:
				difficulty = "Easy";
				break;
			case 1:
				difficulty = "Medium";
				break;
			case 2:
				difficulty = "Hard";
				break;
		}
		if(text != null)
		{
			text.text = difficulty;
		}
	}
}
