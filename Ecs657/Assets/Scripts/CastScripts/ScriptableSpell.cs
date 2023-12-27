using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class ScriptableSpell : ScriptableObject
{
    //projectile game object
    public Transform projectile;
    //particle game object
    public ParticleSystem particleSystem;

    public enum spellClasses { Projectile, Particles, Secret_Third_Option };
    public spellClasses spellClass;

    //the spell description
    public string component;
    //the spell description
    public string description;

    //projectile speed
    public float projSpeed;

    //cooldown on recast
    public float cooldown = 0;

    //damage on contact
    public int damage; 

    //how long a spell effects the enemy
    public int duration = 0;

    //how often the effect of a spell should apply
    public int amo = 1; 
    public int interval = 0;

    //the image that represents the spell
    public Sprite image;

    // evrey posible combination of other spells requierd to cast a spell
    public ScriptableSpell[] combinations = new ScriptableSpell[0];
}
