using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            case NewScriptableSpell.spellClasses.AoE:
                break;
            case NewScriptableSpell.spellClasses.Self:
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
        GameObject currentprojectile = Instantiate(projData.projectile, player.transform.position + player.transform.forward, camera.transform.rotation).gameObject;
        currentprojectile.GetComponent<Rigidbody>().AddForce(camera.transform.forward * projData.projSpeed, ForceMode.Impulse);
        currentprojectile.GetComponent<GenericProjectile>().setData(dmg, spellType.data.duration, spellType.data.interval, playerStats.dmgMul);
    }
    private void Particles()
    {
        NewScriptableSpell.ParticleItem partData = (NewScriptableSpell.ParticleItem) spellType.subdata;
        GameObject flameObj = Instantiate(partData.gameObj, camera.transform).gameObject;
        flameObj.transform.localPosition = new Vector3(0.1f,-0.3f,0f);
        FlamethrowerController particles = flameObj.GetComponent<FlamethrowerController>();
        particles.init(spellType.data.duration,partData.dmgInterval);
        StartCoroutine(particles.StartFlamethrower()); 
    }

    private void AoE()
    {
        NewScriptableSpell.AOEItem aoeData = (NewScriptableSpell.AOEItem) spellType.subdata;
        GameObject aoeObj = Instantiate(aoeData.fieldObj,camera.transform).gameObject;
        //AoE controller = aoeObj.GetComponent<AoE>();
        //StartCoroutine(controller.AoEStart());
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
