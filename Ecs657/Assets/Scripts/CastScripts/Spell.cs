using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Spell : MonoBehaviour
{
    public ScriptableSpell spellType;
    PlayerStats playerStats;
    GameObject player;
    public Sprite icon;
    public ScriptableSpell [] combination;
    public string spellName;
    private bool canDamage = true;

    //interval only functions for repeating effects (e.g. chip damage), so then total duration is duration*interval
    //for one-time effects, (e.g. slowdown), interval is useless so duration is the total time for the effect

    public void InitialiseSpell()
    {
        GameObject camera =GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        combination = spellType.combinations;
        icon = spellType.image;
        spellName = spellType.name;
    }

    //overrides the abstract cast method in spell for this specific spell's behaviour
    public void Cast()
    {
        float dmgMul = playerStats.dmgMul;
        int dmg = Mathf.RoundToInt(spellType.damage*dmgMul);

        //for example, here Fire creates a new fireball from the prefab in fireProj, then adds forward force to it, and initialise its stats
        switch (spellType.spellClass)
        {
            case ScriptableSpell.spellClasses.Projectile:
                Projectile();
                break;
            case ScriptableSpell.spellClasses.Particles:
                Particles();
                break;
            case ScriptableSpell.spellClasses.Secret_Third_Option:
                Debug.Log("test");
                break;
            default:
                break;
        }
    }

    private void Projectile()
    {
        float dmgMul = playerStats.dmgMul;
        int dmg = Mathf.RoundToInt(spellType.damage * dmgMul);
        //for example, here Fire creates a new fireball from the prefab in fireProj, then adds forward force to it, and initialise its stats
        GameObject currentprojectile = Instantiate(spellType.projectile, player.transform.position + player.transform.forward, Quaternion.identity).gameObject;
        currentprojectile.GetComponent<Rigidbody>().AddForce(player.transform.forward * spellType.projSpeed, ForceMode.Impulse);
        currentprojectile.GetComponent<GenericProjectile>().setData(dmg, spellType.duration, spellType.interval);
    }
    private void Particles()
    {
        StartCoroutine(StartFlamethrower(spellType.duration));
    }
    IEnumerator StartFlamethrower(int time)
    {
        //note: currently set to turn off automatically, will later add a check to turn off when player lets go
        spellType.particleSystem.Play();
        yield return new WaitForSeconds(time);
        spellType.particleSystem.Stop();
    }


    void OnParticleCollision(GameObject other)
    {
        if (canDamage)
        {
            int dmg = Mathf.RoundToInt(spellType.damage);
            // Check if the collided object has an "Enemy" component
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Apply damage to the enemy
                enemy.TakeDamage(dmg);
                // Start the damage cooldown coroutine
                StartCoroutine(DamageCooldown());
            }
        }
    }
    IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(spellType.interval);
        canDamage = true;
    }

    //check combination checks if the spell stack contains every element of combination
    //returns indices if it does, null if it doesn't
    public int[] checkCombination(List<Spell> spells)
    {
        if (combination.Length < 2)
        {
            return null;
        }
        List<ScriptableSpell> list = spells.Select(spell => spell.spellType).ToList();
        if (combination.All(spell => list.Contains(spell)))
        {
            int[] indices = combination.Select(spell => list.IndexOf(spell)).ToArray();
            Array.Sort(indices);
            Array.Reverse(indices);
            return indices;
        }
        else
        {
            return null;
        }
    }
}
