using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeableVariable
{
    public string name;
    public float initialValue;
    public float maxValue;
    public float incrementBy;
    public int weight;
    public int actualWeight;
    public bool chosen;
    public Sprite sprite;

    public UpgradeableVariable(string name,
                               ref float initialValue,
                               float maxValue,
                               float incrementBy,
                               int weight,
                               Sprite sprite)
    {
        this.name = name;
        this.initialValue = initialValue;
        this.maxValue = maxValue;
        this.incrementBy = incrementBy;
        this.weight = weight;
        this.sprite = sprite;
        actualWeight = 0;
        chosen = false;
    }
}
