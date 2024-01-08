using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{

    public float timeValue;
    public float totalTimePassed;
    public float statProgressionTime;
    [SerializeField] TextMeshProUGUI label;

    // Start is called before the first frame update
    void Start()
    {
        DisplayTime(timeValue);
    }

	// Update is called once per frame
	void Update()
    {
        totalTimePassed += Time.deltaTime;
        statProgressionTime += Time.deltaTime;
        if (timeValue < 0)
		{   
            return;
		}
        timeValue -= Time.deltaTime;
        DisplayTime(timeValue);
    }

    //display time on text in minutes & seconds
    void DisplayTime(float displayTime)
	{
        float minutes = Mathf.FloorToInt(displayTime / 60);
        float seconds = Mathf.FloorToInt(displayTime % 60);
        if(minutes < 0)
		{
            minutes = 0;
            seconds = 0;
		}
        label.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}
}
