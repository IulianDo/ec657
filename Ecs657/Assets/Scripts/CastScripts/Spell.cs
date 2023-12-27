using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Spell : MonoBehaviour
{
    public NewScriptableSpell spellType;
    PlayerStats playerStats;
    GameObject player;
    GameObject camera;
    public Sprite icon;
    public NewScriptableSpell [] combination;
    public string spellName;
    private bool canDamage = true;
    private int maxAmmo;
    private int ammo;

    //interval only functions for repeating effects (e.g. chip damage), so then total duration is duration*interval
    //for one-time effects, (e.g. slowdown), interval is useless so duration is the total time for the effect

    public void InitialiseSpell()
    {
        camera =GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        combination = spellType.data.combinations;
        icon = spellType.data.image;
        spellName = spellType.name;
        maxAmmo = spellType.data.ammo;
        ammo = maxAmmo;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo; 
    }

    //resets the amonition to maximum
    public void Reload()
    {
        ammo = maxAmmo;
    }

    //overrides the abstract cast method in spell for this specific spell's behaviour
    public int Cast()
    {
        float dmgMul = playerStats.dmgMul;
        int dmg = Mathf.RoundToInt(spellType.data.damage*dmgMul);
        ammo --;
        //for example, here Fire creates a new fireball from the prefab in fireProj, then adds forward force to it, and initialise its stats
        switch (spellType.spellClass)
        {
            case NewScriptableSpell.spellClasses.Projectile:
                Projectile();
                break;
            case NewScriptableSpell.spellClasses.Particles:
                Particles();
                break;
            case NewScriptableSpell.spellClasses.Secret_Third_Option:
                Debug.Log("test");
                break;
            default:
                break;
        }
        return ammo;
    }

    private void Projectile()
    {
        NewScriptableSpell.ProjectileItem projData = (NewScriptableSpell.ProjectileItem) spellType.subdata;
        float dmgMul = playerStats.dmgMul;
        int dmg = Mathf.RoundToInt(spellType.data.damage * dmgMul);
        //for example, here Fire creates a new fireball from the prefab in fireProj, then adds forward force to it, and initialise its stats
        GameObject currentprojectile = Instantiate(projData.projectile, player.transform.position + player.transform.forward, Quaternion.identity).gameObject;
        currentprojectile.GetComponent<Rigidbody>().AddForce(camera.transform.forward * projData.projSpeed, ForceMode.Impulse);
        currentprojectile.GetComponent<GenericProjectile>().setData(dmg, spellType.data.duration, spellType.data.interval);
    }
    private void Particles()
    {
        StartCoroutine(StartFlamethrower(spellType.data.duration));
    }
    IEnumerator StartFlamethrower(int time)
    {
        NewScriptableSpell.ParticleItem partData = (NewScriptableSpell.ParticleItem) spellType.subdata;
        //note: currently set to turn off automatically, will later add a check to turn off when player lets go
        partData.particles.Play();
        yield return new WaitForSeconds(time);
        partData.particles.Stop();
    }


    void OnParticleCollision(GameObject other)
    {
        if (canDamage)
        {
            int dmg = Mathf.RoundToInt(spellType.data.damage);
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
        yield return new WaitForSeconds(spellType.data.interval);
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
        List<NewScriptableSpell> list = spells.Select(spell => spell.spellType).ToList();
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
